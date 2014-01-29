# Zapos

Zapos is the universal table-reporting framework. 
It is supporting two file formats now   - pdf and xlsx. 
Primary document - is the file, based on markup language (like html or xml) with specific page structure. 
Page is formatted using ZCSS style sheets. 

## Page structure

Page structure includes 'style' and 'tables' tags. 
The table has 'tbody' and 'thead' sections, which do not differ from the same sections in CSS.
If page has several tables - each table will be created on a separate Excel sheet.

#### Page example

````html
@model TestReportModel

<style>
    .head {
        font-size: 28pt;
        text-align: center;
    }
    .odd { background-color: #c8d197; }
    .value {
        text-decoration: underline;
        font-style: italic;
        text-align: center;
    }
    .name {
        text-align: left;
        color: #eb2138;
        font-weight: bold;
    }
</style>

<table>
    <thead class="head">
        <tr>
            <th style="width: 50px">Id</th>
            <th style="width: 100px">Name</th>
            <th class="value" style="width: 80px">Value</th>
        </tr>
    </thead>
    <tbody>
        @{ var index = 1; }
        @foreach (var item in Model.Items)
        {
            <tr>
                <td>@item.Id</td>
                <td class="name">@item.Name</td>
                <td class="value">@item.Value</td>
            </tr>

            index++;
        }
    </tbody>
</table>

<table>
    <thead class="head">
        <tr>
            <th style="width: 50px">Id</th>
            <th style="width: 100px">First name</th>
            <th class="value" style="width: 80px">Email</th>
        </tr>
    </thead>
    <tbody>
        @{ index = 1; }
        @foreach (var item in Model.Items)
        {
            <tr class='@(index % 2 == 0 ? "" : "odd")'>
                <td>@item.Id</td>
                <td class="name">@item.FirstName</td>
                <td class="value">@item.Email</td>
            </tr>

            index++;
        }
    </tbody>
</table>
````

### Allowed styles and attributes

#### 1. Attributes:

 * td/th
* formula
* title
* number-format
* colspan
* rowspan

#### 2. Styles:

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

### ZCSS

ZCSS - css-like stylesheets that support only the styles listed above. 
Also, in contrast to CSS, in ZCSS you can only use the class selector.

### How it works

Firstly, you need to include it to the project. This can be done in two ways:
*  Use nuget package manager. 
```nuget
	Install-Package Zapos.Common
	Install-Package Zapos.Constructors.Razor
	Install-Package Zapos.Printers.Gembox
```

*  Manually include the following DLL into project
 * Zapos.Common.dll
 * Zapos.Constructors.Razor.dll
 * Zapos.Printers.Gembox.dll
 
Done! Now you can use this framework in your project. 
How it works you've learnt from the below code example

#### Code example
 
```C#
	var page = HttpContext.Current.Server.MapPath("~/documentForConvert");

    var model = GetModelForMyDocument();

    Func<string, string> pathConverter = HttpContext.Current.Server.MapPath;

    var constructorConfig = new Dictionary<string, object> { { "RESOLVE_PATH_ACTION", pathConverter } };
	
	//main class takes 2 params: gridConstructor and printerFormat(Pdf or Xslx). 
	//Constructor also takes 2 params: constructorConfig and printerConfig
    var report = new Report<RazorGridConstructor, PdfPrinter>(constructorConfig, null);

    var path = Path.GetTempFileName();
	
    using (var stream = new FileStream(path, FileMode.OpenOrCreate))
    {
	//'Create' take 3 params: file stream(your end file), page(path to convertible page) and model
    report.Create(stream, page, model);
	
	return path;
	}
```
 