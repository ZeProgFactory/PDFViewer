# `PDFViewer`

**Namespace :** `ZPF.PDFViewer.Maui`

---

&nbsp;<br/>
## Decimal `CalculatedZoom` ( get;  set;) 

---


&nbsp;<br/>
## int `CurrentPageNumber` ( get;  set;) 

**Summary :**

Gets the current page number in the pagination system.

**Remarks :**

This property is updated internally and raises a property change notification when its value             changes. It is intended for read-only access from outside the class.


---


&nbsp;<br/>
## PDFInfos `Infos` ( get;  set;) 

**Summary :**

Gets the PDF information associated with the document.

**Remarks :**

This property is set internally and is not intended to be modified directly by consumers.             Changes to this property raise the PropertyChanged event, allowing subscribers to be notified when the PDF             information is updated.


---


&nbsp;<br/>
## bool `IsBusy` ( get;  set;) 

**Summary :**

Gets a value indicating whether the component is currently performing a background operation.


---


&nbsp;<br/>
## bool `IsToolbarVisible` ( get;  set;) 

**Summary :**

Gets or sets a value indicating whether the toolbar is visible.

**Remarks :**

Changing this property raises the property changed notification, allowing data bindings or             listeners to respond to visibility changes.


---


&nbsp;<br/>
## string `LastMessage` ( get;  set;) 

---


&nbsp;<br/>
## ObservableCollection<PDFPageInfo> `Pages` ( get;  set;) 

**Summary :**

Gets or sets the collection of pages contained in the PDF document.

**Remarks :**

Each element in the collection represents information about a single page, encapsulated in a             PDFPageInfo instance. Modifying this collection updates the set of pages available in the document in memory.


---


&nbsp;<br/>
## Decimal `ZoomFactor` ( get;  set;) 

---


&nbsp;<br/>
## void `AddLogicalChild` ( Element element ) 

---


&nbsp;<br/>
## void `Arrange` ( Rect bounds ) 

---


&nbsp;<br/>
## void `BatchBegin` (  ) 

---


&nbsp;<br/>
## void `BatchCommit` (  ) 

---


&nbsp;<br/>
## void `CalcZoom` ( PDFPageInfo pageInfo ) 

---


&nbsp;<br/>
## void `ClearLogicalChildren` (  ) 

---


&nbsp;<br/>
## void `ClearPages` (  ) 

**Summary :**

Removes all pages from the collection and updates the associated view to reflect the cleared state.

**Remarks :**

After calling this method, the collection view is reset to an empty state and then repopulated             with the current set of pages. Any data bindings or UI elements dependent on the pages collection will be             refreshed accordingly.


---


&nbsp;<br/>
## void `ClearValue` ( BindableProperty property ) 

---


&nbsp;<br/>
## void `ClearValue` ( BindablePropertyKey propertyKey ) 

---


&nbsp;<br/>
## void `CoerceValue` ( BindableProperty property ) 

---


&nbsp;<br/>
## void `CoerceValue` ( BindablePropertyKey propertyKey ) 

---


&nbsp;<br/>
## Size `CrossPlatformArrange` ( Rect bounds ) 

---


&nbsp;<br/>
## Size `CrossPlatformMeasure` ( Double widthConstraint, Double heightConstraint ) 

---


&nbsp;<br/>
## Task `DoZoom` ( Decimal scale ) 

---


&nbsp;<br/>
## bool `EffectIsAttached` ( string name ) 

---


&nbsp;<br/>
## Object `FindByName` ( string name ) 

---


&nbsp;<br/>
## bool `Focus` (  ) 

---


&nbsp;<br/>
## void `ForceLayout` (  ) 

---


&nbsp;<br/>
## IList<GestureElement> `GetChildElements` ( Point point ) 

---


&nbsp;<br/>
## Object `GetValue` ( BindableProperty property ) 

---


&nbsp;<br/>
## void `InsertLogicalChild` ( int index, Element element ) 

---


&nbsp;<br/>
## void `InvalidateMeasureNonVirtual` ( InvalidationTrigger trigger ) 

---


&nbsp;<br/>
## bool `IsSet` ( BindableProperty targetProperty ) 

---


&nbsp;<br/>
## void `Layout` ( Rect bounds ) 

---


&nbsp;<br/>
## Task `LoadPDF` ( string pdfPath, string password ) 

---


&nbsp;<br/>
## Task<bool> `LoadPDF` ( IPdfSource pdfSource, string url, string password ) 

---


&nbsp;<br/>
## void `LowerChild` ( View view ) 

---


&nbsp;<br/>
## void `LowerChild` ( View view ) 

---


&nbsp;<br/>
## SizeRequest `Measure` ( Double widthConstraint, Double heightConstraint, MeasureFlags flags ) 

---


&nbsp;<br/>
## Size `Measure` ( Double widthConstraint, Double heightConstraint ) 

---


&nbsp;<br/>
## void `PlatformSizeChanged` (  ) 

---


&nbsp;<br/>
## void `RaiseChild` ( View view ) 

---


&nbsp;<br/>
## void `RaiseChild` ( View view ) 

---


&nbsp;<br/>
## void `RemoveBinding` ( BindableProperty property ) 

---


&nbsp;<br/>
## void `RemoveDynamicResource` ( BindableProperty property ) 

---


&nbsp;<br/>
## bool `RemoveLogicalChild` ( Element element ) 

---


&nbsp;<br/>
## ControlTemplate `ResolveControlTemplate` (  ) 

---


&nbsp;<br/>
## Task `SaveFirstPageAsImageAsync` ( string outputImagePath ) 

---


&nbsp;<br/>
## Task `SavePageAsImageAsync` ( string outputImagePath, UInt32 pageNumber ) 

---


&nbsp;<br/>
## void `SetBinding` ( BindableProperty targetProperty, BindingBase binding ) 

---


&nbsp;<br/>
## void `SetDynamicResource` ( BindableProperty property, string key ) 

---


&nbsp;<br/>
## void `SetValue` ( BindableProperty property, Object value ) 

---


&nbsp;<br/>
## void `SetValue` ( BindablePropertyKey propertyKey, Object value ) 

---


&nbsp;<br/>
## void `SetValueFromRenderer` ( BindableProperty property, Object value ) 

---


&nbsp;<br/>
## void `SetValueFromRenderer` ( BindablePropertyKey property, Object value ) 

---


&nbsp;<br/>
## void `Unfocus` (  ) 

---


&nbsp;<br/>
## void `UnloadPDF` (  ) 

---


&nbsp;<br/>
## Task<PDFPageInfo> `UpdatePageInfo` ( PDFPageInfo pageInfo, string outputImagePath ) 

---


