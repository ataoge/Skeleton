using System;
using System.Collections.Generic;
using System.Linq;
using Ataoge.Data.Entities;
using Xunit;

namespace Ataoge.EntityFrameworkCore.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void TestRepostory()
        {
            using(var dbContext = new TestDbContext())
            {
                IList<SequencesRule> ss = dbContext.Set<SequencesRule>().ToList();
            }

        }
    }
}
