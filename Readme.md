<!-- default file list -->
*Files to look at*:

* [DomainObject1.cs](./CS/DXExample.Module/DomainObject1.cs) (VB: [DomainObject1.vb](./VB/DXExample.Module/DomainObject1.vb))
* **[Module.cs](./CS/DXExample.Module/Module.cs) (VB: [Module.vb](./VB/DXExample.Module/Module.vb))**
* [RemoveFromViewInfoAttribute.cs](./CS/DXExample.Module/RemoveFromViewInfoAttribute.cs) (VB: [RemoveFromViewInfoAttribute.vb](./VB/DXExample.Module/RemoveFromViewInfoAttribute.vb))
<!-- default file list end -->
# How to implement a custom attribute to customize the Application Model


<p>This example demonstrates how to use a custom attribute to remove properties from the DetailView.Items and ListView.Columns node. In this case, properties will be removed from the Column Chooser and Layout Customization window, and the user will be able to add them only by using the Add button, placed to these windows in Windows Forms applications.<br />
To implement this functionality, the <strong>IModelMember</strong> interface is extended with the <strong>IRemovedFromViewInfo</strong> interface that contains a single property - <strong>IsRemovedFromViewInfo</strong>. The value of this property is calculated based on the value of the custom attribute (<strong>RemoveFromViewInfoAttribute</strong>) applied to the persistent class. This is done via the domain logic (<strong>RemovedFromViewInfoLogic</strong>). Then the IRemovedFromViewInfo.IsRemovedFromViewInfo property is used by the model nodes generator updater (<strong>ViewsNodesGeneratorUpdater</strong>) to remove certain view items and columns from detail and list views. Thus, the example demonstrates three techniques: extending the Application Model with additional properties, defining default model values via the domain logic and changing the default model via the model nodes generator updater. You can find more information about these techniques in the <a href="https://www.devexpress.com/Support/Center/p/K18252">How to customize the Application Model from the code of the business class</a> article.</p><p><strong>See al</strong><strong>so:</strong><br />
<a href="http://documentation.devexpress.com/#Xaf/CustomDocument3169"><u>Extend and Customize the Application Model in Code</u></a></p>

<br/>


