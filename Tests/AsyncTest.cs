﻿//
// AsyncTest.cs
//
// Author:
//   Eric Maupin <me@ermau.com>
//
// Copyright (c) 2010 Eric Maupin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Linq;
using System.Threading;
using NAssert = NUnit.Framework.Assert;

namespace Tempest.Tests
{
	public class AsyncTest
	{
		private readonly Func<EventArgs, bool> passPredicate;

		public AsyncTest()
		{
			this.passPredicate = e => true;
		}

		public AsyncTest (Func<EventArgs, bool> passPredicate)
		{
			if (passPredicate == null)
				throw new ArgumentNullException ("passPredicate");

			this.passPredicate = passPredicate;
		}

		public void PassHandler (object sender, EventArgs e)
		{
			if (passPredicate (e))
				passed = true;
			else
				failed = true;
		}

		public void FailHandler (object sender, EventArgs e)
		{
			failed = true;
		}

		public void Assert (int timeout)
		{
			DateTime start = DateTime.Now;
			while (DateTime.Now.Subtract (start).TotalMilliseconds < timeout)
			{
				if (failed)
					NAssert.Fail ();
				else if (passed)
					return;

				Thread.Sleep (1);
			}

			NAssert.Fail ("Asynchronous operation timed out.");
		}

		private volatile bool passed = false;
		private volatile bool failed = false;
	}
}