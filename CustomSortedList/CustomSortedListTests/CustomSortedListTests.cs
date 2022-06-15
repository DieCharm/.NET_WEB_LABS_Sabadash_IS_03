using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using CustomSortedList;

namespace CustomSortedListTests
{
    public class CustomSortedListTest
    {
        [Fact]
        public void Constructor_with_parameter_works_correctly()
        {
            int[] array = new[] {7, 2, 12, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            List<int> sorted = array.ToList();
            sorted.Sort();
            int[] arr = new int[check.Count];
            for (int i = 0; i < check.Count; i++)
            {
                arr[i] = check[i];
            }
            Assert.Equal(sorted.ToArray(),arr);
        }
        
        [Fact]
        public void Constructor_without_parameter_works_correctly()
        {
            CustomSortedList<int> check = new CustomSortedList<int>();
            Assert.Equal(new int[0],check);
        }
        
        [Fact]
        public void Reverse_returns_correct_value()
        {
            //arrange
            int[] array = new[] {1, 2, 3, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            
            //act
            int[] arr = check.Reverse.ToArray();
            
            //assert
            Assert.Equal(array.Reverse(),arr);
        }

        [Fact]
        public void Count_returns_correct_value()
        {
            int[] array = new[] {1, 2, 3, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            Assert.Equal(array.Length,check.Count);
        }

        [Fact]
        public void IsReadOnly_returns_correct_value()
        {
            CustomSortedList<int> check = new CustomSortedList<int>();
            Assert.False(check.IsReadOnly);
        }

        [Fact]
        public void Clone_works_correctly()
        {
            CustomSortedList<string> people = new CustomSortedList<string>(new[]
            {
                "str1", "str2", "str3"
            });
            CustomSortedList<string> cloned = (CustomSortedList<string>)people.Clone();
            Assert.Equal(people,cloned);
        }

        [Fact]
        public void Add_works_correctly()
        {
            int[] array = new[] {1, 3, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            check.Add(4);
            Assert.Equal(new []{1, 3, 4, 5}, check);
        }

        [Fact]
        public void Add_throws_ArgumentNullException()
        {
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            string n = null;
            Assert.Throws<ArgumentNullException>((() => check.Add(n)));
        }

        [Fact]
        public void Clear_works_correctly()
        {
            int[] array = new[] {1, 3, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            check.Clear();
            int[] empty = new int[0];
            Assert.Equal(empty,check);
        }

        [Fact]
        public void Contains_returns_true()
        {
            int[] array = new[] {1, 3, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            bool contains = check.Contains(3);
            Assert.True(contains);
        }
        
        [Fact]
        public void Contains_returns_false()
        {
            int[] array = new[] {1, 3, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            bool contains = check.Contains(2);
            Assert.False(contains);
        }

        [Fact]
        public void Contains_throws_ArgumentNullException()
        {
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            string n = null;
            Assert.Throws<ArgumentNullException>((() => check.Contains(n)));
        }

        [Fact]
        public void CopyTo_works_correctly()
        {
            int[] array = new[] {1, 2, 3, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            int[] copied = new int[7];
            check.CopyTo(copied,1);
            Assert.Equal(new [] {0, 1, 2, 3, 4, 5, 0}, copied);
        }

        [Theory]
        [InlineData(5)]
        [InlineData(-1)]
        public void CopyTo_throws_ArgumentOutOfRangeException(int index)
        {
            int[] array = new[] {1, 2, 3, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            int[] copied = new int[7];
            Assert.Throws<ArgumentOutOfRangeException>((() => check.CopyTo(copied, index)));
        }
        
        [Fact]
        public void CopyTo_throws_ArgumentOutOfRangeException_on_minus_parameter()
        {
            int[] array = new[] {1, 2, 3, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            int[] copied = new int[7];
            Assert.Throws<ArgumentOutOfRangeException>((() => check.CopyTo(copied, -1)));
        }

        [Fact]
        public void CopyTo_throws_ArgumentException()
        {
            int[] array = new[] {1, 2, 3, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            int[] copied = new int[5];
            Assert.Throws<ArgumentException>((() => check.CopyTo(copied, 1)));
        }

        [Fact]
        public void Remove_throws_ArgumentNullException()
        {
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            Assert.Throws<ArgumentNullException>((() => check.Remove(null)));
        }

        [Fact]
        public void Remove_changes_repository_and_returns_true()
        {
            string str = "str4";
            string[] array = new[] {"str1", "str2", "str3", str};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            Assert.True(check.Remove(str));
            Assert.Equal(new string[] {"str1", "str2", "str3"}, check);
        }

        [Fact]
        public void Remove_returns_false()
        {
            string str = "str4";
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            Assert.False(check.Remove(str));
            Assert.Equal(array,check);
        }
/// <summary>
/// ////////////////////
/// </summary>
        [Theory]
        [InlineData(7, 4)]
        [InlineData(1, 0)]
        public void IndexOf_returns_correct_value(int item, int expected)
        {
            int[] array = new[] {1, 2, 7, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            Assert.Equal(expected,check.IndexOf(item));
        }

        [Fact]
        public void IndexOf_throws_ArgumentException()
        {
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            Assert.Throws<ArgumentException>(() => check.IndexOf("str"));
        }

        [Fact]
        public void IndexOf_throws_ArgumentNullException()
        {
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            Assert.Throws<ArgumentNullException>(() => check.IndexOf(null));
        }
/// <summary>
/// ////////////////////
/// </summary>
        [Fact]
        public void Insert_throws_ArgumentOutOfRangeException()
        {
            int[] array = new[] {1, 2, 7, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            Assert.Throws<ArgumentOutOfRangeException>((() => check.Insert(6,55)));
        }

        [Fact]
        public void Insert_throws_ArgumentNullException()
        {
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            Assert.Throws<ArgumentNullException>(() => check.Insert(2,null));
        }

        [Fact]
        public void Insert_works_correctly_without_changes()
        {
            string str = "str4";
            string[] array = new[] {"str1", "str2", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            check.Insert(3,str);
            Assert.Equal(new[] {"str1", "str2", "str3", str}, check);
        }
        
        [Fact]
        public void Insert_works_correctly_with_changes_in_collection()
        {
            string str = "str2";
            string[] array = new[] {"str1", "str4", "str3"};
            CustomSortedList<string> check = new CustomSortedList<string>(array);
            check.Insert(0,str);
            Assert.Equal(new[] {"str1", str, "str3", "str4"}, check);
        }

        [Fact]
        public void RemoveAt_works_correctly()
        {
            int[] array = new[] {1, 2, 7, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            check.RemoveAt(4);
            Assert.Equal(new int[] {1, 2, 4, 5}, check);
        }

        [Fact]
        public void RemoveAt_throws_ArgumentOutOfRangeException()
        {
            int[] array = new[] {1, 2, 7, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            Assert.Throws<ArgumentOutOfRangeException>((() => check.RemoveAt(5)));
        }

        [Fact]
        public void Get_works_correctly()
        {
            int[] array = new[] {1, 2, 7, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            Assert.Equal(7,check[4]);
        }

        [Fact]
        public void Get_throws_ArgumentException()
        {
            int[] array = new[] {1, 2, 7, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            Assert.Throws<ArgumentException>(() => check[99]);
        }
        
        [Fact]
        public void Set_throws_ArgumentException()
        {
            int[] array = new[] {1, 2, 7, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            Assert.Throws<ArgumentException>(() => check[99] = 3);
        }
        
        [Fact]
        public void Set_works_correctly()
        {
            int[] array = new[] {1, 2, 3, 4, 5};
            CustomSortedList<int> check = new CustomSortedList<int>(array);
            check[2] = 7;
            Assert.Equal(new int[] {1, 2, 4, 5, 7}, check);
        }
    }
}