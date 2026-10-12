Download the file:
```
view-source:https://api.navisphere.com/api/B2B/Portal/v1/documentation/swagger.yaml
```

Fix: Remove the empty deprecated: property from the OpenAPI schema definition for the 'volume' property in the 'ratingItem' schema. The 'deprecated' property should either be set to true or false, but it cannot be null.
```
"Error converting value {null} to type 'System.Boolean'.
Path 'components.schemas.ratingItem.properties.volume.deprecated', line 1, position 357289."
```