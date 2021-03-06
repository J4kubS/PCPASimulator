﻿//
//  Author:
//    Jakub Šoustar xsoust02@stud.fit.vutbr.cz
//
//  Copyright (c) 2015, Jakub Šoustar
//
//  All rights reserved.
//
//  Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
//
//     * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in
//       the documentation and/or other materials provided with the distribution.
//     * Neither the name of the Jakub Šoustar nor the names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
//
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
//  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
//  A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
//  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
//  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
//  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
//  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
//  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
//  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using System;

namespace PCPASimulator
{
	/// <summary>
	/// Represents errors that occur during the validation of a XML configuration file
	/// for a parallel communicating pushdown automata system.
	/// </summary>
	public class PCPAValidationException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAValidationException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public PCPAValidationException(string message)
			: this(message, 0, 0)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PCPASimulator.PCPAValidationException"/> class.
		/// </summary>
		/// <param name="format">A composite format string.</param>
		/// <param name="lineNumber">Line number indicating where the error occured.</param>
		/// <param name="linePosition">Line position indicating where the error occured.</param>
		public PCPAValidationException(string format, int lineNumber, int linePosition)
			: base(string.Format(format, lineNumber, linePosition))
		{
			LinePosition = linePosition;
			LineNumber = lineNumber;
		}

		/// <summary>
		/// Gets the line number indicating where the error occured.
		/// </summary>
		public int LineNumber{ get; set; }

		/// <summary>
		/// Gets the line position indicating where the error occured.
		/// </summary>
		public int LinePosition { get; set; }
	}
}