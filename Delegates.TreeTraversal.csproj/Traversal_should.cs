using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.TreeTraversal
{
    [TestFixture]
    public class Traversal_should
    {
        public void Test<T,TIntermediate>(IEnumerable<T> actual, 
            Func<T,TIntermediate> selector, 
            params TIntermediate[] expected)
        {
            var a = actual.Select(selector).ToList();
            CollectionAssert.AreEquivalent(expected, a);
        }


        [Test]
        public void GetBinaryTreeValues()
        {
            var data = new BinaryTree<int>
            {
                Value=0,
                Left = new BinaryTree<int>
                {
                    Value=1,
                    Left = new BinaryTree<int>
                    {
                        Value = 3
                    },
                    Right = new BinaryTree<int>
                    {
                        Value= 5,
                        Left = new BinaryTree<int>
                        {
                            Value = 7
                        },
                        Right = new BinaryTree<int>
                        {
                            Value = 9
                        }
                    }
                },
                Right = new BinaryTree<int>
                {
                    Value = 11
                }
            };

            Test(Traversal.GetBinaryTreeValues(data), z => z,0, 1, 3, 5, 7, 9,11);
        }

        [Test]
        public void GetEndJobs()
        {
            var data = new Job
            {
                Name = "4",
                Subjobs = new List<Job>
                  {
                      new Job
                      {
                          Name="3"
                      },
                      new Job
                      {
                          Name="A",
                          Subjobs=new List<Job>
                          {
                              new Job
                              {
                                  Name="1"
                              },
                              new Job
                              {
                                  Name="2"
                              }
                          }

                      }
                  }
            };
            Test(Traversal.GetEndJobs(data), z => z.Name, "1", "2", "3");

        }


        [Test]
        public void GetProducts()
        {
            var data = new ProductCategory
            {
                Categories = new List<ProductCategory>
                 {
                     new ProductCategory
                     {
                         Products=new List<Product>
                         {
                             new Product
                             {
                                  Name="X"
                             },
                             new Product
                             {
                                 Name="Y"
                             }
                         }
                     },
                     new ProductCategory
                     {
                          Products=new List<Product>
                          {
                               new Product
                               {
                                   Name="1"
                               }
                          }
                     }
                 },
                Products = new List<Product>
                  {
                      new Product
                      {
                          Name="A"
                      }
                  }
            };

            Test(Traversal.GetProducts(data), z => z.Name, "X", "Y", "1", "A");
        }
    }
}
