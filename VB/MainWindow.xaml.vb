Imports System.Windows
Imports System.Collections.ObjectModel
Imports DevExpress.Xpf.Grid

Namespace DXTreeListSample
	Partial Public Class MainWindow
		Inherits Window

		Private Index As Integer = 10
		Public Property Items() As ObservableCollection(Of SomeObject)
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
		Public Sub New(ByVal name As String, ByVal parentId As Integer, ByVal id As Integer)
			Me.Id = id
			Me.ParentId = parentId
			Me.Name = name
		End Sub
		Public Property Id() As Integer
		Public Property ParentId() As Integer
		Public Property Name() As String
	End Class
End Namespace