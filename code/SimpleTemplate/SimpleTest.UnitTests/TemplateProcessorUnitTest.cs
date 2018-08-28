using System;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SimpleTest.UnitTests
{
    [TestClass]
    public class TemplateProcessorUnitTest
    {
        #region attributes
        private JObject _Data = JObject.Parse("{'ID':1,'Operation':'store changed','OldStore':{'Stores':['Lambton Quay','Willis Street'],'Manufacturers':[{'Name':'Acme Co','Products':[{'Name':'Anvil','Price':50}]},{'Name':'Contoso','Products':[{'Name':'Elbow Grease','Price':99.95},{'Name':'Headlight Fluid','Price':4},{'Name':'Milk','Price':2}]}]},'NewStore':{'Stores':['Lambton Quay','Willis Street','Shobra Street'],'Manufacturers':[{'Name':'Acme Company','Products':[{'Name':'Anvil','Price':50},{'Name':'Beans','Price':10,'Available':true}]},{'Name':'Contoso','Products':[{'Name':'Elbow Grease','Price':99.95}]}]}}");

        private JObject _Data2 = JObject.Parse("{'@Id':0,'@Operation':'CustomerOrderUpdated','@Date':'2018-07-3013: 51: 00','@Reason':'CustomerOrderUpdated','@Source':'CustomerOrderUpdated','@UCID':'9999000009','@ActionByUsername':'admin','@PurchaseOrderNumber':'PON_12345678','@PurchaseOrderNumberID':30,'CustomerProfile':{'@ID':10013,'@Title':'Ms','@BusinessTitle':null,'@UCID':'9999000009','@Username':'User124','@BirthDate':'2018-05-2200: 00: 00','@Gender':'Male','@ProfileType':'Subscriber','@IsCorporate':false,'@CorporateDueDay':null,'@Status':'Active','@Reason':'Active','@GeographicalRegion':'Begam','@OrganizationHierarchy':null,'@CSOOrganizationHierarchy':'Maadi','@Nationality':'Egypt','@MembershipDate':'2018-05-2900: 00: 00','@Comments':'','@Occupation':'Manager','@CompanyActivity':'Communications','@ProfileCreatorID':1,'@NationalID':'','@PassportID':'','NameList':{'Name':[{'@Language':'English','@FirstName':'User','@MiddleName':'2','@LastName':'4'}]},'EmailList':{'Email':[{'@Email':'User124@xyz.com','@EmailType':'Preferred'}]},'TelephoneList':{'Telephone':[{'@TelephoneNo':'0212345678','@TelephoneType':'HomeTelephone'}]},'AddressList':{'Address':[{'@Language':'English','@Address1':'cairo','@Address2':'','@ZipCode':''}]},'ContactList':{'Contact':[]},'ProfileRelationsList':{'ProfileRelation':[{'@RelationType':'hasparent','@ProfileID':10,'@ProfileUCID':'9999000003'}]},'ProfileCategoryList':{'ProfileCategory':[{'@ProfileCategoryName':'BC_Initiative2011'}]}},'OldCustomerOrder':{'@ID':24,'@BusinessStatus':'New','@PurchaseOrderNumber':'PON_12345678','@PurchaseOrderNumberID':30,'@PurchaseDate':'2018-07-2511: 13: 51','@ResellerUsername':'','@ResellerProfileID':null,'@ResellerUCID':'','@ResellerType':'','@PaymentTerm':1,'@ContractDuration':12,'@SalesAllocation':'Roma','@BillingLocation':'CUSTOMER','@IsCommitment':false,'@PaymentMethod':'Cash','@ProfileID':10013,'@AccountManagerID':1,'@AccountManagerName':'administrator','@SaleCreatorID':1,'@SaleCreatorName':'administrator','@UCID':'9999000009','CustomerOrderItemList':{'CustomerOrderItem':[{'@ID':32,'@CustomerOrderID':24,'@ProductOfferingID':5,'@ProductOfferingName':'StarterSolar','@ProductOfferingFamilyID':5,'@ProductOfferingFamilyName':'Solar','@ProductCatalogID':1,'@ProductCatalogName':'StandardConsumers','@ActivationPrice':'120','@InstallmentPrice':'120','DiscountList':{'Discount':[{'@ID':21,'@CustomerOrderItemID':32,'@AmountInPercent':'50','@Precedence':2,'@DiscountSpecificationID':1,'@DiscountSpecificationName':'FamilyandFriends','@MainCount':null,'@RemainingCount':null,'@IsActive':true,'@StartDate':'2018-07-2511: 14: 24','@EndDate':'2028-07-2511: 14: 24'},{'@ID':22,'@CustomerOrderItemID':32,'@AmountInPercent':'10','@Precedence':2,'@DiscountSpecificationID':2,'@DiscountSpecificationName':'VIP','@MainCount':null,'@RemainingCount':null,'@IsActive':true,'@StartDate':'2018-07-2511: 14: 24','@EndDate':'2028-07-2511: 14: 24'}]},'ProductList':{'Product':[{'@ID':32,'@CustomerOrderItemID':32,'@Type':'SolarEnergy','@ProductSpecificationID':4,'@ProductSpecificationName':'StarterSolar','@ProvisionStatus':'NotProvisioned','@IsBillingActive':false,'@BillingStartDate':null,'@BillingEndDate':null,'@Recurring':'Rent','@FactoryID':103}]}}]}},'NewCustomerOrder':{'@ID':24,'@BusinessStatus':'New','@PurchaseOrderNumber':'PON_12345678','@PurchaseOrderNumberID':30,'@PurchaseDate':'2018-07-2511: 13: 51','@ResellerUsername':'','@ResellerProfileID':null,'@ResellerUCID':'','@ResellerType':'','@PaymentTerm':1,'@ContractDuration':12,'@SalesAllocation':'Roma','@BillingLocation':'CUSTOMER','@IsCommitment':true,'@PaymentMethod':'Cash','@ProfileID':10013,'@AccountManagerID':1,'@AccountManagerName':'administrator','@SaleCreatorID':1,'@SaleCreatorName':'administrator','@UCID':'9999000009','CustomerOrderItemList':{'CustomerOrderItem':[{'@ID':32,'@CustomerOrderID':24,'@ProductOfferingID':5,'@ProductOfferingName':'StarterSolar','@ProductOfferingFamilyID':5,'@ProductOfferingFamilyName':'Solar','@ProductCatalogID':1,'@ProductCatalogName':'StandardConsumers','@ActivationPrice':'120','@InstallmentPrice':'120','DiscountList':{'Discount':[{'@ID':21,'@CustomerOrderItemID':32,'@AmountInPercent':'95','@Precedence':2,'@DiscountSpecificationID':1,'@DiscountSpecificationName':'FamilyandFriends','@MainCount':null,'@RemainingCount':null,'@IsActive':true,'@StartDate':'2018-07-2511: 14: 24','@EndDate':'2028-07-2511: 14: 24'},{'@ID':23,'@CustomerOrderItemID':32,'@AmountInPercent':'20','@Precedence':2,'@DiscountSpecificationID':3,'@DiscountSpecificationName':'Corporate','@MainCount':null,'@RemainingCount':null,'@IsActive':true,'@StartDate':'2018-07-2511: 14: 24','@EndDate':'2028-07-2511: 14: 24'}]},'ProductList':{'Product':[{'@ID':32,'@CustomerOrderItemID':32,'@Type':'SolarEnergy','@ProductSpecificationID':4,'@ProductSpecificationName':'StarterSolar','@ProvisionStatus':'NotProvisioned','@IsBillingActive':false,'@BillingStartDate':null,'@BillingEndDate':null,'@Recurring':'Rent','@FactoryID':103}]}}]}}}");
        private JObject _Data3 = JObject.Parse(@"{'@Id':0,'@Operation':'CustomerOrderUpdated','@Date':'2018-07-3013: 51: 00','@Reason':'CustomerOrderUpdated','@Source':'CustomerOrderUpdated','@UCID':'9999000009','@ActionByUsername':'admin','@PurchaseOrderNumber':'PON_12345678','@PurchaseOrderNumberID':30,'CustomerProfile':{'@ID':10013,'@Title':'Ms','@BusinessTitle':null,'@UCID':'9999000009','@Username':'User124','@BirthDate':'2018-05-2200: 00: 00','@Gender':'Male','@ProfileType':'Subscriber','@IsCorporate':false,'@CorporateDueDay':null,'@Status':'Active','@Reason':'Active','@GeographicalRegion':'Begam','@OrganizationHierarchy':null,'@CSOOrganizationHierarchy':'Maadi','@Nationality':'Egypt','@MembershipDate':'2018-05-2900: 00: 00','@Comments':'','@Occupation':'Manager','@CompanyActivity':'Communications','@ProfileCreatorID':1,'@NationalID':'','@PassportID':'','NameList':{'Name':[{'@Language':'English','@FirstName':'User','@MiddleName':'2','@LastName':'4'}]},'EmailList':{'Email':[{'@Email':'User124@xyz.com','@EmailType':'Preferred'}]},'TelephoneList':{'Telephone':[{'@TelephoneNo':'0212345678','@TelephoneType':'HomeTelephone'}]},'AddressList':{'Address':[{'@Language':'English','@Address1':'cairo','@Address2':'','@ZipCode':''}]},'ContactList':{'Contact':[]},'ProfileRelationsList':{'ProfileRelation':[{'@RelationType':'hasparent','@ProfileID':10,'@ProfileUCID':'9999000003'}]},'ProfileCategoryList':{'ProfileCategory':[{'@ProfileCategoryName':'BC_Initiative2011'}]}},'OldCustomerOrder':{'@ID':24,'@BusinessStatus':'New','@PurchaseOrderNumber':'PON_12345678','@PurchaseOrderNumberID':30,'@PurchaseDate':'2018-07-2511: 13: 51','@ResellerUsername':'','@ResellerProfileID':null,'@ResellerUCID':'','@ResellerType':'','@PaymentTerm':1,'@ContractDuration':12,'@SalesAllocation':'Roma','@BillingLocation':'CUSTOMER','@IsCommitment':false,'@PaymentMethod':'Cash','@ProfileID':10013,'@AccountManagerID':1,'@AccountManagerName':'administrator','@SaleCreatorID':1,'@SaleCreatorName':'administrator','@UCID':'9999000009','CustomerOrderItemList':{'CustomerOrderItem':[{'@ID':32,'@CustomerOrderID':24,'@ProductOfferingID':5,'@ProductOfferingName':'StarterSolar','@ProductOfferingFamilyID':5,'@ProductOfferingFamilyName':'Solar','@ProductCatalogID':1,'@ProductCatalogName':'StandardConsumers','@ActivationPrice':'120','@InstallmentPrice':'120','DiscountList':{'Discount':[{'@ID':21,'@CustomerOrderItemID':32,'@AmountInPercent':'50','@Precedence':2,'@DiscountSpecificationID':1,'@DiscountSpecificationName':'FamilyandFriends','@MainCount':null,'@RemainingCount':null,'@IsActive':true,'@StartDate':'2018-07-25 11: 14: 24','@EndDate':'2028-07-25 11: 14: 24'},{'@ID':22,'@CustomerOrderItemID':32,'@AmountInPercent':'10','@Precedence':2,'@DiscountSpecificationID':2,'@DiscountSpecificationName':'VIP','@MainCount':null,'@RemainingCount':null,'@IsActive':true,'@StartDate':'2018-07-25 11: 14: 24','@EndDate':'2028-07-25 11: 14: 24'}]},'ProductList':{'Product':[{'@ID':32,'@CustomerOrderItemID':32,'@Type':'SolarEnergy','@ProductSpecificationID':4,'@ProductSpecificationName':'StarterSolar','@ProvisionStatus':'NotProvisioned','@IsBillingActive':false,'@BillingStartDate':null,'@BillingEndDate':null,'@Recurring':'Rent','@FactoryID':103}]}}]}},'NewCustomerOrder':{'@ID':24,'@BusinessStatus':'New','@PurchaseOrderNumber':'PON_12345678','@PurchaseOrderNumberID':30,'@PurchaseDate':'2018-07-2511: 13: 51','@ResellerUsername':'','@ResellerProfileID':null,'@ResellerUCID':'','@ResellerType':'','@PaymentTerm':1,'@ContractDuration':12,'@SalesAllocation':'Roma','@BillingLocation':'CUSTOMER','@IsCommitment':true,'@PaymentMethod':'Cash','@ProfileID':10013,'@AccountManagerID':1,'@AccountManagerName':'administrator','@SaleCreatorID':1,'@SaleCreatorName':'administrator','@UCID':'9999000009','CustomerOrderItemList':{'CustomerOrderItem':[{'@ID':32,'@CustomerOrderID':24,'@ProductOfferingID':5,'@ProductOfferingName':'StarterSolar','@ProductOfferingFamilyID':5,'@ProductOfferingFamilyName':'Solar','@ProductCatalogID':1,'@ProductCatalogName':'StandardConsumers','@ActivationPrice':'120','@InstallmentPrice':'120','DiscountList':{'Discount':[]},'ProductList':{'Product':[{'@ID':32,'@CustomerOrderItemID':32,'@Type':'SolarEnergy','@ProductSpecificationID':4,'@ProductSpecificationName':'StarterSolar','@ProvisionStatus':'NotProvisioned','@IsBillingActive':false,'@BillingStartDate':null,'@BillingEndDate':null,'@Recurring':'Rent','@FactoryID':103}]}}]}}}");
        #endregion

        [TestMethod]
        public void TestNoFunction()
        {
            string template = "An event of {{Operation}} submitted.";
            
            string expected = "An event of store changed submitted.";

            SimpleTemplate.Business.TemplateProcesser processer = new SimpleTemplate.Business.TemplateProcesser(template, _Data);

            Assert.AreEqual(expected, processer.Result);
        }

        [TestMethod]
        public void TestMapFunction()
        {
            string template = "An event of {{$Map$Operation}} submitted.";
            Hashtable mapping = new Hashtable
            {
                { "store changed", "store modified" }
            };

            string expected = "An event of store modified submitted.";

            SimpleTemplate.Business.TemplateProcesser processer = new SimpleTemplate.Business.TemplateProcesser(template, _Data, mapping);

            Assert.AreEqual(expected, processer.Result);
        }

        [TestMethod]
        public void TestCompareFunction1()
        {
            string template = "{{@ActionByUsername}} modified the sale with sale order number {{OldCustomerOrder.@PurchaseOrderNumber}} through \"{{$Map$@Source}}\". The following are the list of the changes:\r\n{{$Compare$OldCustomerOrder,NewCustomerOrder}}";

            Hashtable mapping = new Hashtable
            {
                { "CustomerOrderUpdated", "Tracer website" }
            };

            string expected = "admin modified the sale with sale order number PON_12345678 through \"Tracer website\". The following are the list of the changes:\r\n\r\n1. \"is commitment\" changed from False to True\r\n2. Discount modified \"amount in percent\" from 50 to 95\r\n3. Discount removed with the criteria: [ID is 22, \"amount in percent\" is 10, discount specification ID is 2, discount specification name is VIP]\r\n4. Discount added with the criteria: [ID is 23, \"amount in percent\" is 20, discount specification ID is 3, discount specification name is Corporate]\r\n\r\n";

            SimpleTemplate.Business.TemplateProcesser processer = new SimpleTemplate.Business.TemplateProcesser(template, _Data2, mapping);

            Assert.AreEqual(expected, processer.Result);
        }

        [TestMethod]
        public void TestCompareFunction2()
        {
            string template = "{{@ActionByUsername}} modified the sale with sale order number {{OldCustomerOrder.@PurchaseOrderNumber}} through \"{{$Map$@Source}}\". The following are the list of the changes:\r\n{{$Compare$OldCustomerOrder,NewCustomerOrder}}";

            Hashtable mapping = new Hashtable
            {
                { "CustomerOrderUpdated", "Tracer website" }
            };

            string expected = "admin modified the sale with sale order number PON_12345678 through \"Tracer website\". The following are the list of the changes:\r\n\r\n1. \"is commitment\" changed from False to True\r\n2. Discount removed with the criteria: [ID is 21, customer order item ID is 32, \"amount in percent\" is 50, precedence is 2, discount specification ID is 1, discount specification name is FamilyandFriends, no main count, no remaining count, \"is active\" is True, start date is Wed, 25 Jul, 2018 11:14 AM, end date is Tue, 25 Jul, 2028 11:14 AM]\r\n3. Discount removed with the criteria: [ID is 22, customer order item ID is 32, \"amount in percent\" is 10, precedence is 2, discount specification ID is 2, discount specification name is VIP, no main count, no remaining count, \"is active\" is True, start date is Wed, 25 Jul, 2018 11:14 AM, end date is Tue, 25 Jul, 2028 11:14 AM]\r\n\r\n";

            SimpleTemplate.Business.TemplateProcesser processer = new SimpleTemplate.Business.TemplateProcesser(template, _Data3, mapping);

            Assert.AreEqual(expected, processer.Result);
        }
    }
}
