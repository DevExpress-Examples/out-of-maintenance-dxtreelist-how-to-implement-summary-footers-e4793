Imports Microsoft.VisualBasic
Imports System.Windows
Imports System.Collections.ObjectModel
Imports DevExpress.Xpf.Grid

Namespace DXTreeListSample
	Partial Public Class MainWindow
		Inherits Window
		Private Index As Integer = 10
		Private privateItems As ObservableCollection(Of SomeObject)
		Public Property Items() As ObservableCollection(Of SomeObject)
			Get
				Return privateItems
			End Get
			Set(ByVal value As ObservableCollection(Of SomeObject))
				privateItems = value
			End Set
		End Property
		Public Sub New()
			Items = New ObservableCollection(Of SomeObject)()
			For i As Integer = 0 To 9
				Items.Add(New SomeObject(String.Format("First level - {0}", i), i, i))
				For j As Integer = 0 To 9
					Items.Add(New SomeObject(String.Format("First level - {0}.{1}", i, j), i, Index))
					Index += 1
				Next j
			Next i
			InitializeComponent()
			grid.ItemsSource = Items
		End Sub
	End Class
	Public Class SomeObject
		Public Sub New()
		End Sub
        Public Sub New(ByVal _name As String, ByVal _parentId As Integer, ByVal _id As Integer)
            Id = _id
            ParentId = _parentId
            Name = _name
        End Sub
		Private privateId As Integer
		Public Property Id() As Integer
			Get
				Return privateId
			End Get
			Set(ByVal value As Integer)
				privateId = value
			End Set
		End Property
		Private privateParentId As Integer
		Public Property ParentId() As Integer
			Get
				Return privateParentId
			End Get
			Set(ByVal value As Integer)
				privateParentId = value
			End Set
		End Property
		Private privateName As String
		Public Property Name() As String
			Get
				Return privateName
			End Get
			Set(ByVal value As String)
				privateName = value
			End Set
        End Property
        Public Overrides Function ToString() As String
            Return Id.ToString()
        End Function
	End Class
End Namespace