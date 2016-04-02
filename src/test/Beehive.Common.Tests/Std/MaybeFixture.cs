using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Beehive.Common.Std;
using NUnit.Framework;

namespace Beehive.Common.Tests.Std
{
    [TestFixture]
    public class MaybeFixture
    {
        [Test(Description = "Maybe effect should be able to lift a non-null value into the context")]
        public void Maybe_should_lift_not_null_value_into_the_ctx()
        {
            const string value = "some string";

            var context = Maybe.ApplyTo(value);

            Assert.That(context.HasValue(), Is.EqualTo(true));
            Assert.That(context.GetValue(), Is.EqualTo(value));
        }

        [Test(Description = "Maybe effect should be able to lift a null value into the context")]
        public void Maybe_should_lift_null_value_into_the_ctx()
        {
            string value = null;

            var context = Maybe.ApplyTo(value);

            Assert.That(context.HasValue(), Is.EqualTo(false));
        }

        [Test(Description = "Maybe container should thrown an exception on attempt to access value in None projection")]
        public void Maybe_should_throw_an_exception_while_trying_to_extract_value_from_nothing()
        {
            string value = null;

            var context = Maybe.ApplyTo(value);

            Assert.Throws<NoneException>(() => context.GetValue());
        }

        [Test(Description = "Maybe container should be able to fallback to default value or computation on attempt to access value in None projection")]
        public void Maybe_should_fallback_to_default_value_when_extracting_data_from_nothing()
        {
            const string defaultValue = "default value";

            string value = null;

            var context = Maybe.ApplyTo(value);

            Assert.That(context.HasValue(), Is.EqualTo(false));
            Assert.Throws<NoneException>(() => context.GetValue());
            Assert.That(context.GetValueOr(defaultValue), Is.EqualTo(defaultValue));
        }

        [Test(Description = "Maybe container should be able to return primary value on attempt to access value in Some projection")]
        public void Maybe_should_get_primary_value_when_extracting_from_something()
        {
            const string defaultValue = "default value";

            string value = "another value";

            var context = Maybe.ApplyTo(value);

            Assert.That(context.HasValue(), Is.EqualTo(true));
            Assert.That(context.GetValueOr(defaultValue), Is.EqualTo(value));
        }

        [Test(Description = "Maybe container should support monadic comprehensions")]
        public void Maybe_should_be_representable_as_monadic_comprehension()
        {
            const string defaultString = "__defaultString";
            const int defaultInteger = 42;

            var ctx = from a in Maybe.ApplyTo(defaultInteger)
                      from b in Maybe.ApplyTo(defaultString)
                      select new { Left = a, Right = b };

            Assert.That(ctx.HasValue(), Is.EqualTo(true));
            Assert.That(ctx.GetValue().Left, Is.EqualTo(defaultInteger));
            Assert.That(ctx.GetValue().Right, Is.EqualTo(defaultString));
        }
    }
}
