using System.Windows;
using System.Collections.ObjectModel;
using DevExpress.Xpf.Grid;

namespace DXTreeListSample {
    public partial class MainWindow: Window {
        int Index = 10;
        public ObservableCollection<SomeObject> Items { get; set; }
        public MainWindow() {
            Items = new ObservableCollection<SomeObject>();
            for (int i = 0; i < 10; i++) {
                Items.Add(new SomeObject(string.Format("First level - {0}", i), i, i));
                for (int j = 0; j < 10; j++)
                    Items.Add(new SomeObject(string.Format("First level - {0}.{1}", i, j), i, Index++));
            }
            InitializeComponent();
			grid.ItemsSource = Items;
        }
    }
    public class SomeObject {
        public SomeObject() { }
        public SomeObject(string name, int parentId, int id) {
            Id = id;
            ParentId = parentId;
            Name = name;
        }
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
    }
}