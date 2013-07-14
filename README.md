# WebCache

Proxy-friendly web caching.

Available on NuGet: https://nuget.org/packages/WebCache/

## Usage
```c#
WebCache.Register(
	new Bundle
	{
		Name = "Header",
		Assets = new List<Asset>
		{
			new Asset("/assets/styles/main.min.css"),
			new Asset("/assets/styles/app.min.css"),
			new Asset("/assets/scripts/header.js")
		}
	},
	new Bundle
	{
		Name = "Body",
		Assets = new List<Asset>
		{
			new Asset("/assets/scripts/log.js")
		}
	}
);

```html
@using WebCache
<!DOCTYPE html>
<html>
<head>
	<title>WebCache</title>
	@Html.RenderBundle("Header")
</head>
<body>
	<h1>WebCache</h1>
	<a href="/">Request page again</a>
	@Html.RenderBundle("Body")
</body>
</html>


