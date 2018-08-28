using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace SimpleTest.UnitTests
{
    [TestClass]
    public class DescribeJsonCompUnitTest
    {
        /// 1. modify property at the root [@IsCommitment]
        /// 2. remove an object inside other objects [Discount]
        /// 3. add an object inside other objects [Discount]
        /// 4. Update property in an object [@AmountInPercent]
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
            string expected = "\r\n1. \"is commitment\" changed from False to True\r\n2. Discount modified \"amount in percent\" from 50 to 95\r\n3. Discount removed with the criteria: [ID is 22, \"amount in percent\" is 10, discount specification ID is 2, discount specification name is VIP]\r\n4. Discount added with the criteria: [ID is 23, \"amount in percent\" is 20, discount specification ID is 3, discount specification name is Corporate]\r\n\r\n";
          
            string result = SimpleTemplate.Business.Helper.JsonHelper.DescribeJsonComparison(patch);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// 1. modify property at the root [@IsCommitment]
        /// 2. remove 2 objects inside other objects [Discount]
        /// </summary>
        [TestMethod]
        public void TestDescribeJsonComparison2()
        {
            JToken patch = JToken.Parse(@"
{
  'CustomerOrderItemList': {
    'CustomerOrderItem': {
      '_t': 'a',
      '0': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '0': {
              '@AmountInPercent': [
                '100',
                '50'
              ]
            }
          }
        }
      },
      '1': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '_0': [
              {
                '@ID': 13,
                '@CustomerOrderItemID': 16,
                '@AmountInPercent': '0',
                '@Precedence': 2,
                '@DiscountSpecificationID': 1,
                '@DiscountSpecificationName': 'Family and Friends',
                '@MainCount': null,
                '@RemainingCount': null,
                '@IsActive': true,
                '@StartDate': '2018-06-05 11:08:45',
                '@EndDate': '2028-06-05 11:08:45'
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
            string expected = "\r\n1. Discount modified \"amount in percent\" from 100 to 50\r\n2. Discount removed with the criteria: [ID is 13, customer order item ID is 16, \"amount in percent\" is 0, precedence is 2, discount specification ID is 1, discount specification name is Family and Friends, no main count, no remaining count, \"is active\" is True, start date is Tue, 5 Jun, 2018 11:08 AM, end date is Mon, 5 Jun, 2028 11:08 AM]\r\n\r\n";
          
            string result = SimpleTemplate.Business.Helper.JsonHelper.DescribeJsonComparison(patch);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// 1. add object inside an object
        /// 2. remove object inside an object
        /// </summary>
        [TestMethod]
        public void TestDescribeJsonComparison3()
        {
            JToken patch = JToken.Parse(@"
{
  'CustomerOrderItemList': {
    'CustomerOrderItem': {
      '_t': 'a',
      '0': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '0': [
              {
                '@ID': 12,
                '@CustomerOrderItemID': 15,
                '@AmountInPercent': '50',
                '@Precedence': 2,
                '@DiscountSpecificationID': 1,
                '@DiscountSpecificationName': 'Family and Friends',
                '@MainCount': null,
                '@RemainingCount': null,
                '@IsActive': true,
                '@StartDate': '2018-06-05 11:08:45',
                '@EndDate': '2028-06-05 11:08:45'
              }
            ]
          }
        }
      },
      '1': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '_0': [
              {
                '@ID': 13,
                '@CustomerOrderItemID': 16,
                '@AmountInPercent': '0',
                '@Precedence': 2,
                '@DiscountSpecificationID': 1,
                '@DiscountSpecificationName': 'Family and Friends',
                '@MainCount': null,
                '@RemainingCount': null,
                '@IsActive': true,
                '@StartDate': '2018-06-05 11:08:45',
                '@EndDate': '2028-06-05 11:08:45'
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
            string expected = "\r\n1. Discount added with the criteria: [ID is 12, customer order item ID is 15, \"amount in percent\" is 50, precedence is 2, discount specification ID is 1, discount specification name is Family and Friends, no main count, no remaining count, \"is active\" is True, start date is Tue, 5 Jun, 2018 11:08 AM, end date is Mon, 5 Jun, 2028 11:08 AM]\r\n2. Discount removed with the criteria: [ID is 13, customer order item ID is 16, \"amount in percent\" is 0, precedence is 2, discount specification ID is 1, discount specification name is Family and Friends, no main count, no remaining count, \"is active\" is True, start date is Tue, 5 Jun, 2018 11:08 AM, end date is Mon, 5 Jun, 2028 11:08 AM]\r\n\r\n";
          
            string result = SimpleTemplate.Business.Helper.JsonHelper.DescribeJsonComparison(patch);

            Assert.AreEqual(expected, result);
        }

        /// <summary>
        /// 1. add object and remove object on the same level
        /// </summary>
        [TestMethod]
        public void TestDescribeJsonComparison4()
        {
            JToken patch = JToken.Parse(@"
{
  'CustomerOrderItemList': {
    'CustomerOrderItem': {
      '_t': 'a',
      '0': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '0': {
              '@ID': [
                12,
                53
              ],
              '@AmountInPercent': [
                '100',
                '50'
              ],
              '@DiscountSpecificationName': [
                'Family and Friends',
                'VIP'
              ]
            }
          }
        }
      },
      '1': {
        'DiscountList': {
          'Discount': {
            '_t': 'a',
            '_0': [
              {
                '@ID': 13,
                '@CustomerOrderItemID': 16,
                '@AmountInPercent': '0',
                '@Precedence': 2,
                '@DiscountSpecificationID': 1,
                '@DiscountSpecificationName': 'Family and Friends',
                '@MainCount': null,
                '@RemainingCount': null,
                '@IsActive': true,
                '@StartDate': '2018-06-05 11:08:45',
                '@EndDate': '2028-06-05 11:08:45'
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
            string expected = "\r\n1. Discount removed with the criteria: [ID is 12, \"amount in percent\" is 100, discount specification name is Family and Friends]\r\n2. Discount added with the criteria: [ID is 53, \"amount in percent\" is 50, discount specification name is VIP]\r\n3. Discount removed with the criteria: [ID is 13, customer order item ID is 16, \"amount in percent\" is 0, precedence is 2, discount specification ID is 1, discount specification name is Family and Friends, no main count, no remaining count, \"is active\" is True, start date is Tue, 5 Jun, 2018 11:08 AM, end date is Mon, 5 Jun, 2028 11:08 AM]\r\n\r\n";
          
            string result = SimpleTemplate.Business.Helper.JsonHelper.DescribeJsonComparison(patch);

            Assert.AreEqual(expected, result);
        }
    }
}
