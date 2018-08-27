using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SimpleTest.UnitTests
{
    [TestClass]
    public class DescribeJsonCompUnitTest
    {
        [TestMethod]
        public void TestDescribeJsonComparison1()
        {
            JToken patch = JToken.Parse(@"
{
  '@IsCommitment': [
    false,
    true
  ],
  'CustomerOrderItemList': {
    'CustomerOrderItem': {
      '_t': 'a',
      '0': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '0': {
              '@AmountInPercent': [
                '50',
                '95'
              ]
            },
            '1': {
              '@ID': [
                22,
                23
              ],
              '@AmountInPercent': [
                '10',
                '20'
              ],
              '@DiscountSpecificationID': [
                2,
                3
              ],
              '@DiscountSpecificationName': [
                'VIP',
                'Corporate'
              ]
            }
          }
        }
      }
    }
  }
}
");
            string expected = @"
1. @IsCommitment changed from false to true
2. There is a change in CustomerOrderItemList
3. There is a change in the CustomerOrderItem list
4. There is a change in DiscountList
5. There is a change in the Discount list
6. @AmountInPercent changed from 50 to 95
7. Discount removed with the criteria:
    1. @ID is 22
    2. @AmountInPercent is 10
    3. @DiscountSpecificationID is 2
    4. @DiscountSpecificationName is VIP
8. Discount added with the criteria:
    1. @ID is 23
    2. @AmountInPercent is 20
    3. @DiscountSpecificationID is 3
    4. @DiscountSpecificationName is Corporate
";
          
            string result = SimpleTemplate.Business.Helper.JsonHelper.DescribeJsonComparison(patch);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDescribeJsonComparison2()
        {
            JToken patch = JToken.Parse(@"
{
  '@IsCommitment': [
    false,
    true
  ],
  'CustomerOrderItemList': {
    'CustomerOrderItem': {
      '_t': 'a',
      '0': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '_0': [
              {
                '@ID': 21,
                '@CustomerOrderItemID': 32,
                '@AmountInPercent': '50',
                '@Precedence': 2,
                '@DiscountSpecificationID': 1,
                '@DiscountSpecificationName': 'FamilyandFriends',
                '@MainCount': null,
                '@RemainingCount': null,
                '@IsActive': true,
                '@StartDate': '2018-07-2511: 14: 24',
                '@EndDate': '2028-07-2511: 14: 24'
              },
              0,
              0
            ],
            '_1': [
              {
                '@ID': 22,
                '@CustomerOrderItemID': 32,
                '@AmountInPercent': '10',
                '@Precedence': 2,
                '@DiscountSpecificationID': 2,
                '@DiscountSpecificationName': 'VIP',
                '@MainCount': null,
                '@RemainingCount': null,
                '@IsActive': true,
                '@StartDate': '2018-07-2511: 14: 24',
                '@EndDate': '2028-07-2511: 14: 24'
              },
              0,
              0
            ]
          }
        }
      }
    }
  }
}");
            string expected = @"
1. @IsCommitment changed from false to true
2. There is a change in CustomerOrderItemList
3. There is a change in the CustomerOrderItem list
4. There is a change in DiscountList
5. There is a change in the Discount list
6. @AmountInPercent changed from 50 to 95
7. Discount removed with the criteria:
    1. @ID is 22
    2. @AmountInPercent is 10
    3. @DiscountSpecificationID is 2
    4. @DiscountSpecificationName is VIP
8. Discount added with the criteria:
    1. @ID is 23
    2. @AmountInPercent is 20
    3. @DiscountSpecificationID is 3
    4. @DiscountSpecificationName is Corporate
";
          
            string result = SimpleTemplate.Business.Helper.JsonHelper.DescribeJsonComparison(patch);

            Assert.AreEqual(expected, result);
        }
    }
}
