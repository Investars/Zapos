## Zapos

Zapos, is the universal table-reporting framework. He is supporting now  two file formats  - pdf and xlsx. Primary document - is file, based on markup language (like html or xml) with specific page structure.


#### Page structure

—труктура страницы состоит тега <style> ... </style>, включающего в себ€ ZCSS стили и тегов <table> ... </table>.
Table поддерживает thead и tbody, которые ничем не отличаютс€ от html таблиц.

#### Allowed styles and attributes

###### 1. Attributes:

  * td/th
* formula
* title
* number-format

###### 2. Styles:

* color: #(hex)
* text-align: center | left | right
* text-decoration: [line-through | overline | underline] | none
* vertical-align: bottom | middle | top | justify
* width: (dec)px
* height: (dec)px
* background-color: #(hex)
* border-left: (dec)px [none | dotted | dashed | solid | double] #(hex)
* border-right: (dec)px [none | dotted | dashed | solid | double] #(hex)
* border-top: (dec)px [none | dotted | dashed | solid | double] #(hex)
* border-bottom: (dec)px [none | dotted | dashed | solid | double] #(hex)
* font-family: name
* font-size: (dec)pt
* font-style: normal | italic
* font-weight: bold | normal

#### ZCSS

ZCSS - css-подобные таблицы стилей, которые поддерживают только перечисленные ранее стили. “ак же, в отличии от CSS, в ZCSS возможно использование только селектора по классу.

#### »спользование

ƒл€ конвертации документа из ML в PDF или XLSX служит метод Create(Stream stream, string filePath, T model) Report<TGridConstructor, TPrinter>. TPrinter - класс, который содержит параметры принтера (PdfPrinter либо XlsxPrinter). TGridConstructor



