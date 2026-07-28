/*
 * Copyright © 2020-2026 Dieter (coder2000) Lunn
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 *
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Shouldly;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Xunit;

namespace Ubiety.Dns.Test
{
    /// <summary>
    /// Question equality is the response cache's key, so these comparisons decide whether a cached
    /// answer is found or silently missed.
    /// </summary>
    public class QuestionEqualityTests
    {
        private static Question Q(
            string name = "example.com",
            QuestionType type = QuestionType.A,
            QuestionClass cls = QuestionClass.IN) => new(name, type, cls);

        [Fact]
        public void QuestionsWithTheSameFieldsAreEqual()
        {
            var left = Q();
            var right = Q();

            left.Equals(right).ShouldBeTrue();
            left.Equals((object)right).ShouldBeTrue();
            (left == right).ShouldBeTrue();
            (left != right).ShouldBeFalse();
            left.GetHashCode().ShouldBe(right.GetHashCode());
        }

        [Fact]
        public void DomainNameComparisonIsCaseInsensitive()
        {
            // DNS names are case insensitive on the wire, so the cache must not miss on casing.
            Q("EXAMPLE.COM").Equals(Q("example.com")).ShouldBeTrue();
        }

        [Fact]
        public void TheTrailingSeparatorDoesNotAffectEquality()
        {
            Q("example.com.").Equals(Q("example.com")).ShouldBeTrue();
        }

        [Fact]
        public void ADifferentNameIsNotEqual()
        {
            Q("example.com").Equals(Q("example.org")).ShouldBeFalse();
        }

        [Fact]
        public void ADifferentQuestionTypeIsNotEqual()
        {
            Q(type: QuestionType.A).Equals(Q(type: QuestionType.MX)).ShouldBeFalse();
        }

        [Fact]
        public void ADifferentQuestionClassIsNotEqual()
        {
            Q(cls: QuestionClass.IN).Equals(Q(cls: QuestionClass.CH)).ShouldBeFalse();
        }

        [Fact]
        public void AQuestionEqualsItself()
        {
            var question = Q();

            question.Equals(question).ShouldBeTrue();
            question.Equals((object)question).ShouldBeTrue();
        }

        [Fact]
        public void NullIsNeverEqual()
        {
            var question = Q();

            question.Equals(null).ShouldBeFalse();
            question.Equals((object?)null).ShouldBeFalse();
            (question == null).ShouldBeFalse();
            (null == question).ShouldBeFalse();
            (question != null).ShouldBeTrue();
        }

        [Fact]
        public void TwoNullsAreEqual()
        {
            Question? left = null;
            Question? right = null;

            (left == right).ShouldBeTrue();
            (left != right).ShouldBeFalse();
        }

        [Fact]
        public void ADifferentTypeIsNotEqual()
        {
            Q().Equals("example.com").ShouldBeFalse();
        }
    }
}
