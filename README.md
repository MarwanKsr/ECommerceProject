# Gateway
### Ocelot
* Added Authentication and Authorization to Gateway

# Class Library
* Added common base classes, interfaces, and libraries

# Services
## EmailApi
* FluentEmail library was added to send users mails after a success order by smtp provider
## IdentityApi
* Microsoft JWT Bearer library was added to add Authentication and Authorization to Project
### EndPoints:
* Login
* Register 
## OrderApi
### EndPoints:
* MakeOrderSuccess
## PaymentApi
* Iyzipay library was added to recieve payment from clients
## ProductApi
* TwentyTwenty library was added to save product images in local
### EndPoints:
* GetProducts
* CreateProduct
* UpdateProduct
* DeleteProduct
* UpdateImage
* GetPrice
* DecreaseStock
## ShoppingCardApi
### EndPoints:
* CreateCard
* RemoveCard
* GetCard
* ClearCard
* CheckOut
