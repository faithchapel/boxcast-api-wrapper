## BoxCast .NET API Wrapper
### Basic Use
#### Authentication

```csharp
//supply API credentials to the API wrapper
BoxCast.SetAPICredentials(clientId, secret);

//authenticate using the oAuth password flow
await BoxCast.Authenticate(username, password);
```

#### Get a Single Resource
```csharp
// Create a request to send to the API
var request = new BoxCastRequest()
{
    Resource = "account"
};

// Request the resource from the API
BoxCastResponse<Dictionary<string, object>> account;
account = await BoxCast.Get<Dictionary<string, object>>(request);
```
    
#### Get a List of Resources

```csharp
// Create a request to send to the API
var request = new BoxCastRequest()
{
	Resource = "broadcasts"
};

// Request the resources from the API
BoxCastResponse<List<Dictionary<string, object>>> broadcasts;
broadcasts = await BoxCast.Get<List<Dictionary<string, object>>>(request);
```

#### Request Parameters

```csharp
var request = new BoxCastRequest()
{
	Resource = "broadcasts"
	Search = "timeframe:future",
	SortBy = "starts_at",
	SortDescending = true,
	PageNumber = 1,
	PageSizeLimit = 5,
	ETag = entityTag        
};

//add undocumented or expanded query parameters
request.QueryParameters.Add("since", "2018-02-03T00:00:00-05:00");
request.QueryParameters.Add("until", "2018-02-04T00:00:00-05:00");
```
    
#### Create a Resource

```csharp
var content = new Dictionary<string, object>();

BoxCastResponse<Dictionary<string, object>> response;
response = await BoxCast.Post("Broadcasts/" + broadcastId, content);
```

#### Update a Resource

```csharp
var content = new Dictionary<string, object>();
    
BoxCastResponse<Dictionary<string, object>> response;
response = await BoxCast.Put("Broadcasts/" + broadcastId, content);
```

#### Delete a Resource

```csharp
await BoxCast.Delete("Broadcasts/" + broadcastId);
```

#### API Responses
Responses returned from the api come as a `BoxCastResponse<T>` object with the following information:

**ETag**  - an entity tag returned from the api for making conditional requests using the ETag field on requests
**Pagination** - an object containing pagination data
- **First**
- **Previous**
- **Next**
- **Last**
- **Total**

**RateLimit** - an object containing rate limit related data
- **Limit**
- **Remaining**
- **Reset**

**Headers** - a dictionary of all headers returned with the response
**ResponseCode** - the HTTP response code returned
**ResponsePhrase** - the response phrase returned
**Content** - the content returned deserialized into the supplied type for the generic response

#### Error Handling
```csharp
try
{
	if(!BoxCast.HasToken)
	{
		await BoxCast.Authenticate(username, password);
	}

	// Do some things...

}
catch(BoxCastAPIException ex)
{
	if (ex.BoxCastError.Error == "invalid_token" || ex.BoxCastError.Error == "unauthorized")
	{
		BoxCast.ClearToken(); //clear the oAuth token so we'll grab a new one next time.
	}
}
```