## Zapos

Zapos, is the universal table-reporting framework. He is supporting now  two file formats  - pdf and xlsx. Primary document - is file, based on markup language (like html or xml) with specific page structure.


#### Page structure

��������� �������� ������� ���� <style> ... </style>, ����������� � ���� ZCSS ����� � ����� <table> ... </table>.
Table ������������ thead � tbody, ������� ����� �� ���������� �� html ������.

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

ZCSS - css-�������� ������� ������, ������� ������������ ������ ������������� ����� �����. ��� ��, � ������� �� CSS, � ZCSS �������� ������������� ������ ��������� �� ������.

#### �������������

��� ����������� ��������� �� ML � PDF ��� XLSX ������ ����� Create(Stream stream, string filePath, T model) Report<TGridConstructor, TPrinter>. TPrinter - �����, ������� �������� ��������� �������� (PdfPrinter ���� XlsxPrinter). TGridConstructor



