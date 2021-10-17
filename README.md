***Please check considerations at the end.***

# refactor-this

The attached project is a poorly written products API in C#.

Please evaluate and refactor areas where you think can be improved.

Consider all aspects of good software engineering and show us how you'll make it #beautiful and make it a production
ready code.

## Getting started for applicants

There should be these endpoints:

1. `GET /products` - gets all products.
2. `GET /products?name={name}` - finds all products matching the specified name.
3. `GET /products/{id}` - gets the project that matches the specified ID - ID is a GUID.
4. `POST /products` - creates a new product.
5. `PUT /products/{id}` - updates a product.
6. `DELETE /products/{id}` - deletes a product and its options.
7. `GET /products/{id}/options` - finds all options for a specified product.
8. `GET /products/{id}/options/{optionId}` - finds the specified product option for the specified product.
9. `POST /products/{id}/options` - adds a new product option to the specified product.
10. `PUT /products/{id}/options/{optionId}` - updates the specified product option.
11. `DELETE /products/{id}/options/{optionId}` - deletes the specified product option.

All models are specified in the `/Models` folder, but should conform to:

**Product:**

```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description",
  "Price": 123.45,
  "DeliveryPrice": 12.34
}
```

**Products:**

```
{
  "Items": [
    {
      // product
    },
    {
      // product
    }
  ]
}
```

**Product Option:**

```
{
  "Id": "01234567-89ab-cdef-0123-456789abcdef",
  "Name": "Product name",
  "Description": "Product description"
}
```

**Product Options:**

```
{
  "Items": [
    {
      // product option
    },
    {
      // product option
    }
  ]
}
```

## Considerations

1. Before making any changes in the code, it should be tested enough to guarantee these changes do no break existing
   logic or business implementations.
2. The refactoring was done taking a DDD approach, especially given the original models were already feature-rich
   instead of POCOs. However, data access responsibility was extracted and moved to a service in the Data layer.
3. I organized Domain and Data layers in folders for simplicity and as a first step in this refactoring process. I would
   expect that, at the end of the process, they would become separate projects.
4. Also considering the above, we moved away from `Models` and now have a Domain. Models should not have any logic
   inside and are mainly used for data transfer.  
   For this exercise, it does not seem necessary to have it since we can make use of attributes to flag what is exposed
   or isn't, so we do not need to be converting a domain object to a model. This decision depends on how the software
   will evolve, so if we see there will be a benefit in also having models we can easily add them.
5. I did not separate ProductOptions from Product endpoints in the Controller like I did in the domain simply because
   they make sense within the same route `\products`. In the event that this controller would increase too much in the
   future it might then be worth it breaking down in that manner.
6. I worked on this exercise for about 1.5 hours, so I have not refactored everything. However, what has been done to
   the `Products` domain object can essentially be replicated to the other ones for a complete production-ready
   solution.   
   I believe working this out on one of the domain objects is enough to show my approach to the problem and reach the
   expectation, without having to spend many hours for the sake of this assignment.

## Final Thoughts

1. Refactoring this whole solution would take a considerable amount of time, especially because we should have
   everything well tested before making changes. Also, some classes used are not trivial to mock for testing.  
   I focused on a small subset in `Product` to showcase how it should be done.
2. I agree that the domain should be feature rich, but most of what is inside the models was data logic. These should be
   moved to respective services in the data layer.
3. It is understandable that this could be legacy code to some extent, but at the end of the refactoring I would replace
   the existing data access with a proper ORM, such as Entity Framework Core.  
   Even if it becomes necessary to run queries from T-SQL, EF can accommodate that so there is no need to make use of a
   DataReader IMO.
