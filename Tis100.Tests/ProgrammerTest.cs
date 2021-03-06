// <copyright file="ProgrammerTest.cs">Copyright ©  2018</copyright>
using System;
using System.Collections.Generic;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tis100;

namespace Tis100.Tests
{
    /// <summary>This class contains parameterized unit tests for Programmer</summary>
    [PexClass(typeof(Programmer))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ProgrammerTest
    {
        /// <summary>Test stub for program(TisCore, List`1&lt;String&gt;)</summary>
        [PexMethod]
        public bool programTest(
            [PexAssumeUnderTest]Programmer target,
            TisCore core,
            List<string> lines
        )
        {
            bool result = target.program(core, lines);
            return result;
            // TODO: add assertions to method ProgrammerTest.programTest(Programmer, TisCore, List`1<String>)
        }
    }
}
