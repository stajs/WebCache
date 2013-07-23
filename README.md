# WebCache

Proxy-friendly web caching.

Available on NuGet: https://nuget.org/packages/WebCache/

## Usage

Register assets on application start:

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
```

Render bundles in your HTML:

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
```

Which produces:

```html

<!DOCTYPE html>
<html>
<head>
	<title>WebCache</title>
	<link rel="stylesheet" href="/assets/styles/main.min.1d343cdc458745e8b092421272c3acd2.webcache.css" />
	<link rel="stylesheet" href="/assets/styles/app.min.aaf81fa9b555358807d986b3b225a06b.webcache.css" />
	<script src="/assets/scripts/header.d0844ec6fc5c98cda897d740b3840337.webcache.js" /></script>
</head>
<body>
	<h1>WebCache</h1>
	<a href="/">Request page again</a>
	<script src="/assets/scripts/log.37309cb1fc49681c1c20becff68312c6.webcache.js" /></script>
</body>
</html>
```

Add the HttpHandler to your web.config:

```xml
<system.webServer>
	<handlers>
		<add name="WebCache" verb="*" path="*.webcache.*" type="WebCache.WebCacheHttpHandler" preCondition="managedHandler"/>
	</handlers>
</system.webServer>
```

Kick back and relax, knowing your assests are sent compressed with long-lived cache headers.