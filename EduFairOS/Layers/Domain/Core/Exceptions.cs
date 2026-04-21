
using System;

namespace EduFairOS.Layers.Domain.Core
{
	public class EduFairException : Exception
	{
		public EduFairException() : base() { }
		public EduFairException(string message) : base(message) { }
		public EduFairException(string message, Exception innerException) : base(message, innerException) { }
	}
}
