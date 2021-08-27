<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/128657717/19.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/E4793)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->
<!-- default file list -->
*Files to look at*:

* [MainWindow.xaml](./CS/MainWindow.xaml) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))
* [MainWindow.xaml.cs](./CS/MainWindow.xaml.cs) (VB: [MainWindow.xaml](./VB/MainWindow.xaml))
* [MyTreeListView.xaml](./CS/MyTreeListView.xaml) (VB: [MyTreeListView.xaml](./VB/MyTreeListView.xaml))
* [MyTreeListView.xaml.cs](./CS/MyTreeListView.xaml.cs) (VB: [MyTreeListView.xaml](./VB/MyTreeListView.xaml))
* [TreeListSummaryItem.cs](./CS/TreeListSummaryItem.cs) (VB: [TreeListSummaryItem.vb](./VB/TreeListSummaryItem.vb))
<!-- default file list end -->
# DXTreeList - How to implement summary footers


<p>At the moment, our TreeListView does not provide the capability to show <a href="http://documentation.devexpress.com/#WindowsForms/CustomDocument1070"><u>summary footers</u></a> for its nodes. This example demonstrates how to implement this functionality manually by using a custom <strong>DataRowTemplate</strong>.<br /><br />This workaround has a few limitations:<br /> 1. It supports only a two-level structure.<br />2. The context menu for summary footers is not supported.<br />3. The grid's horizontal scrolling is not supported.</p>

<br/>


