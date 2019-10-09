using DevExpress.Data;
using DevExpress.Data.TreeList;
using DevExpress.Xpf.Data;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.Native;
using DevExpress.Xpf.Grid.TreeList;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DXTreeListSample {
	public partial class MyTreeListView: TreeListView {
		public MyTreeListView() {
			InitializeComponent();
		}
		protected override TreeListDataProvider CreateDataProvider() {
			return new MyTreeListDataProvider(this);
		}
		public MyTreeListDataProvider PublicDataProvider { get { return (MyTreeListDataProvider)DataProviderBase; } }
		public event EventHandler<EventArgs> NodeSummaryUpdated;
		public void RaiseNodeSummaryUpdated() {
			if (NodeSummaryUpdated != null)
				NodeSummaryUpdated(this, new EventArgs());
		}
	}
	public class MyTreeListDataProvider: TreeListDataProvider {
		public MyTreeListDataProvider(TreeListView view) : base(view) { }
        protected override TreeListDataController CreateDataController() {
            return new MyTreeListDataController(this);
        }
	}
    public class MyTreeListDataController : TreeListDataController {
        public MyTreeListDataController(MyTreeListDataProvider provider) : base(provider) {
        }
        public new MyTreeListDataProvider DataProvider { get { return (MyTreeListDataProvider)base.DataProvider; } }

        protected override void CalcSummary(IEnumerable<DevExpress.Data.TreeList.TreeListSummaryItem> summaryItems, Dictionary<TreeListNodeBase, SummaryDataItem> summaryData, IEnumerable<TreeListNodeBase> nodes, IEnumerable<TreeListNodeBase> selectedNodes = null)
        {
            base.CalcSummary(summaryItems, summaryData, nodes, selectedNodes);
            UpdateGroupSummaries();
        }
        
        protected override void UpdateTotalSummaryOnNodeCollectionChanged(TreeListNodeBase node, NodeChangeType changeType, IEnumerable<DevExpress.Data.TreeList.TreeListSummaryItem> changedItems, Dictionary<TreeListNodeBase, SummaryDataItem> summaryData) {
            base.UpdateTotalSummaryOnNodeCollectionChanged(node, changeType, changedItems, summaryData);
            UpdateGroupSummaries();
        }
        void UpdateGroupSummaries() {
            foreach(TreeListNode current in SummaryData.Keys) {
                var collection = new List<TreeListSummaryValue>();
                SummaryDataItem summaryItem = SummaryData[current];
                foreach(TreeListSummaryValue summaryValue in summaryItem.Values) {
                    collection.Add(summaryValue);
                }
                current.Tag = collection;
            }
            ((MyTreeListView)DataProvider.View).RaiseNodeSummaryUpdated();
        }
    }
	public class GroupFooter: Control {
		public static readonly DependencyProperty RowHandleProperty =
			DependencyProperty.Register("RowHandle", typeof(int), typeof(GroupFooter), new PropertyMetadata(-1, OnRowHandlePropertyChanged));

		static void OnRowHandlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e){
			((GroupFooter)d).OnRowHandleChanged(e);
		}

		public GroupFooter() {
			DefaultStyleKey = typeof(GroupFooter);
			DataContextChanged += OnDataContextChanged;
		}

		#region Properties
		public int RowHandle {
			get { return (int)GetValue(RowHandleProperty); }
			set { SetValue(RowHandleProperty, value); }
		}
		public MyTreeListView View { get { return DataContext == null ? null : (MyTreeListView)((RowData)DataContext).View; } }
		public TreeListControl Grid { get { return (TreeListControl)View.DataControl; } }
		public TreeListRowData RowData { get { return (TreeListRowData)DataContext; } }
		#endregion

		void OnRowHandleChanged(DependencyPropertyChangedEventArgs e) {
			RefreshContent();
		}
		void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
			View.NodeSummaryUpdated += OnViewNodeSummaryIsUpdated;
			Grid.Columns.CollectionChanged += OnColumnsCollectionChanged;
			SubscribeColumns();
		}
		DependencyPropertyDescriptor d = DependencyPropertyDescriptor.FromProperty(ColumnBase.VisibleProperty, typeof(ColumnBase));
		void OnColumnsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
			RefreshContent();
			SubscribeColumns();
		}
		void OnColumnVisibleChanged(object sender, EventArgs e) {
			RefreshContent();
		}
		void OnViewNodeSummaryIsUpdated(object sender, EventArgs e) {
			RefreshContent();
		}

		void SubscribeColumns() {
			foreach (var column in Grid.Columns) {
				d.RemoveValueChanged(column, OnColumnVisibleChanged);
				d.AddValueChanged(column, OnColumnVisibleChanged);
			}
		}
		public void RefreshContent() {
			if (View == null)
				return;
			TreeListNode parent = View.GetNodeByRowHandle(RowHandle).ParentNode;
			if (parent == null)
				UpdateVisibility(-1);
			else {
				UpdateVisibility(parent.RowHandle);
				if (Visibility == Visibility.Visible)
					UpdateGroupFooterSummaryContent(parent.RowHandle);
			}
		}
		void UpdateGroupFooterSummaryContent(int groupRowHandle) {
			List<TreeListSummaryValue> summaryItems = RowData.Node.ParentNode.Tag as List<TreeListSummaryValue>;
			if (summaryItems == null)
				return;
			Dictionary<ColumnBase, string> summaryValues = new Dictionary<ColumnBase, string>();
			for (int i = 0; i < View.VisibleColumns.Count; i++)
				summaryValues.Add(View.VisibleColumns[i], string.Empty);
			foreach (var value in summaryItems) {

                DevExpress.Xpf.Grid.TreeListSummaryItem item = value.SummaryItem.Tag as DevExpress.Xpf.Grid.TreeListSummaryItem;
				if(!item.Visible)
					continue;
				TreeListColumn col = Grid.Columns[item.FieldName];
				if (!summaryValues.ContainsKey(col))
					continue;
				if (!string.IsNullOrEmpty(summaryValues[col]))
					summaryValues[Grid.Columns[item.FieldName]] += "\n";
				summaryValues[Grid.Columns[item.FieldName]] += item.SummaryType + " = " + value.Value;
			}
			Tag = summaryValues;
		}
		void UpdateVisibility(int groupRowHandle) {
			if (groupRowHandle == -1)
				Visibility = Visibility.Collapsed;
			else
				Visibility = IsLastRowInGroup(groupRowHandle) ? Visibility.Visible : Visibility.Collapsed;
		}
		public bool IsLastRowInGroup(int groupRowHandle) {
			TreeListNode node = View.GetNodeByRowHandle(groupRowHandle);
			int maxHandle = 0;
			for (int i = 0; i < node.Nodes.Count; i++) {
				TreeListNode n = node.Nodes[i];
				if (n.RowHandle > maxHandle)
					maxHandle = n.RowHandle;
			}
			return RowHandle == maxHandle;
		}
	}
	public class Conv: MarkupExtension, IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value is string && !string.IsNullOrEmpty((string)value) ? Visibility.Visible : Visibility.Hidden;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			throw new NotImplementedException();
		}
		public override object ProvideValue(IServiceProvider serviceProvider) {
			return this;
		}
	}
}