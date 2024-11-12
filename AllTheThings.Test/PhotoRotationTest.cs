using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AllTheThings.Test
{
	public class PhotoRotationTest
	{
		public static void GetImageMetadataTest()
		{
			string inputImagePath = @"./Images/portrait.jpg";//@"C:\tmp\IDPaaS\debug\portrait.jpg";
			string outputImagePath = "C:\\tmp\\IDPaaS\\debug\\portrait_rotate.jpg";

			// Load image
			using (Image inputImage = Image.Load(inputImagePath))
			{
				var exifProfile = inputImage.Metadata?.ExifProfile;
				var values = exifProfile?.Values;
				if (values != null)
				{
					PrintExifValues(values);

					//exifProfile!.TryGetValue(ExifTag.Orientation, out var orientationValue);
					//exifProfile!.TryGetValue(ExifTag.PixelXDimension, out var widthValue);
					//exifProfile!.TryGetValue(ExifTag.PixelYDimension, out var heightValue);

					//if (widthValue!.Value > heightValue!.Value && orientationValue!.Value == 6)
					if (exifProfile!.TryGetValue(ExifTag.Orientation, out var orientationValue))
					{
						if (orientationValue.Value == 6)
						{
							inputImage.Mutate(x => x.Rotate(90));
							inputImage.Metadata!.ExifProfile!.SetValue(ExifTag.Orientation, (ushort)1);
							inputImage.Save(outputImagePath);

							Image outputImage = Image.Load(outputImagePath);
							var oExifProfile = outputImage.Metadata?.ExifProfile;
							var oValues = oExifProfile?.Values;
							if (oValues != null) PrintExifValues(oValues);
						}
					}
				}
			}
		}

		private  static void PrintExifValues(IReadOnlyList<IExifValue> values)
		{
			foreach (var value in values)
			{
				Console.WriteLine($"Tag: {value.Tag}, Value: {value.GetValue()}, DataType: {value.DataType.ToString()}");
			}
			Console.WriteLine($"====================================================");
		}
	}
}
