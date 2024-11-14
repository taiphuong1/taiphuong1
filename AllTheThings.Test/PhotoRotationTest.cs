using SixLabors.ImageSharp.Metadata.Profiles.Exif;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace AllTheThings.Test
{
	public class PhotoRotationTest
	{
		public static void GetImageMetadataTest()
		{
			string inputImagePath = @"./Images/2.jpg";//@"C:\tmp\IDPaaS\photo\iphone\IMG_6059.jpeg";
			string outputImagePath = "./Images/2_updated.jpg";// "C:\\tmp\\IDPaaS\\photo\\iphone\\updated.jpg";

			// Load image
			using (Image inputImage = Image.Load(inputImagePath))
			{
				var exifProfile = inputImage.Metadata?.ExifProfile;
				var values = exifProfile?.Values;
				bool isImageUpdated = false;
				if (values != null)
				{
					PrintExifValues(values);

					//exifProfile!.TryGetValue(ExifTag.Orientation, out var orientationValue);
					//exifProfile!.TryGetValue(ExifTag.PixelXDimension, out var widthValue);
					//exifProfile!.TryGetValue(ExifTag.PixelYDimension, out var heightValue);
					//if (widthValue!.Value > heightValue!.Value && orientationValue!.Value == 6)

					if (exifProfile!.TryGetValue(ExifTag.Orientation, out var orientationValue))
					{
						switch (orientationValue.Value)
						{
							case 1: //Horizontal (normal)
								break;
							case 2: //Mirror horizontal
								inputImage.Mutate(x => x.Flip(FlipMode.Horizontal));
								isImageUpdated = true;
								break;
							case 3: //Rotate 180 CW
								inputImage.Mutate(x => x.Rotate(180));
								isImageUpdated = true;
								break;
							case 4: //Mirror vertical
								inputImage.Mutate(x => x.Flip(FlipMode.Vertical));
								isImageUpdated = true;
								break;
							case 5: //Mirror horizontal and rotate 270 CW
								inputImage.Mutate(x => x.RotateFlip(RotateMode.Rotate270, FlipMode.Vertical)); //Rotate first then Flip
								isImageUpdated = true;
								break;
							case 6: //Rotate 90 CW
								inputImage.Mutate(x => x.Rotate(90));
								isImageUpdated = true;
								break;
							case 7: //Mirror horizontal and rotate 90 CW
								inputImage.Mutate(x => x.RotateFlip(RotateMode.Rotate90, FlipMode.Vertical)); //Rotate first then Flip
								isImageUpdated = true;
								break;
							case 8: //Rotate 270 CW
								inputImage.Mutate(x => x.Rotate(270));
								isImageUpdated = true;
								break;
							default:
								break;
						}

						if (isImageUpdated)
						{
							inputImage.Metadata!.ExifProfile!.SetValue(ExifTag.Orientation, (ushort)1); //set it back to normal
							inputImage.Save(outputImagePath);

							//Re-print the Exif metadata
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
