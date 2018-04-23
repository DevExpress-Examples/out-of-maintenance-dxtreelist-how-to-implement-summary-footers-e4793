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
		Public Sub New(ByVal name As String, ByVal parentId As Integer, ByVal id As Integer)
            m_Id = id
            m_ParentId = parentId
            m_Name = name
		End Sub
        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(value As Integer)
                m_Id = Value
            End Set
        End Property
        Private m_Id As Integer
        Public Property ParentId() As Integer
            Get
                Return m_ParentId
            End Get
            Set(value As Integer)
                m_ParentId = Value
            End Set
        End Property
        Private m_ParentId As Integer
        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Set(value As String)
                m_Name = Value
            End Set
        End Property
        Private m_Name As String
	End Class
End Namespace