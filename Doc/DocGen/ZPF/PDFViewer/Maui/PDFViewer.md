# `PDFViewer`

**Namespace :** `ZPF.PDFViewer.Maui`

---

## `ClearPages`

**Résumé :**

Removes all pages from the collection and updates the associated view to reflect the cleared state.

**Remarques :**

After calling this method, the collection view is reset to an empty state and then repopulated             with the current set of pages. Any data bindings or UI elements dependent on the pages collection will be             refreshed accordingly.

---


## `CurrentPageNumber`

**Résumé :**

Gets the current page number in the pagination system.

**Remarques :**

This property is updated internally and raises a property change notification when its value             changes. It is intended for read-only access from outside the class.

---


## `Infos`

**Résumé :**

Gets the PDF information associated with the document.

**Remarques :**

This property is set internally and is not intended to be modified directly by consumers.             Changes to this property raise the PropertyChanged event, allowing subscribers to be notified when the PDF             information is updated.

---


## `IsBusy`

**Résumé :**

Gets a value indicating whether the component is currently performing a background operation.

---


## `IsToolbarVisible`

**Résumé :**

Gets or sets a value indicating whether the toolbar is visible.

**Remarques :**

Changing this property raises the property changed notification, allowing data bindings or             listeners to respond to visibility changes.

---


## `Pages`

**Résumé :**

Gets or sets the collection of pages contained in the PDF document.

**Remarques :**

Each element in the collection represents information about a single page, encapsulated in a             PDFPageInfo instance. Modifying this collection updates the set of pages available in the document in memory.

---


