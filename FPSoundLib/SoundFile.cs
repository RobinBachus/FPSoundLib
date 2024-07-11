using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPSoundLib
{
	public abstract class SoundFile
	{
		protected byte[] FileBuffer;

		internal SoundFile(byte[] fileBuffer)
		{
			FileBuffer = fileBuffer;
		}
	}
}
