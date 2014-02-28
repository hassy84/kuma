using System.Collections;

public class Matrix {

	public static double[,] LaplacianOfGaussian {
		get {
			return new double[,] {
				{  0,  0, -1,  0,  0 },
				{  0, -1, -2, -1,  0 },
				{ -1, -2, 16, -2, -1 },
				{  0, -1, -2, -1,  0 },
				{  0,  0, -1,  0,  0 }
			};
		}
	}

	public static double[,] GuruguruMarble {
		get {
			return new double[,] {
				{  0,  0, 0.4,  0,  0 },
				{  0, 0.4, 0.8, 0.4,  0 },
				{ 0.4, 0.8, 1, 0.5, 0.4 },
				{  0, 0.4, 0.8, 0.4,  0 },
				{  0,  0, 0.4,  0,  0 }
			};
		}
	}

	public static float[] ConvolutionFilter(float[] sourcePixelMap,
	                                        double[,] filterMatrix,
	                                        int width,
	                                        int height,
	                                        float factor = 1f,
	                                        int bias = 0) {

		float[] resultPixelMap = new float[sourcePixelMap.Length];

		float alpha = 0.0f;

		int filterWidth = filterMatrix.GetLength(1);
		int filterHeight = filterMatrix.GetLength(0);

		int filterOffset = (filterWidth - 1) / 2;
		int calcOffset = 0;

		int byteOffset = 0;

		for (int offsetY = filterOffset; offsetY < height - filterOffset; ++offsetY) {
			for (int offsetX = filterOffset; offsetX < width - filterOffset; ++offsetX) {
				alpha = 0;

				byteOffset = offsetY *
				             width +
				             offsetX;

				for (int filterY = -filterOffset; filterY <= filterOffset; ++filterY) {
					for (int filterX = -filterOffset; filterX <= filterOffset; ++filterX) {

						calcOffset = byteOffset +
						             filterX +
						             filterY * width;

						alpha += (float)sourcePixelMap[calcOffset] *
						         (float)filterMatrix[filterY + filterOffset,
						                             filterX + filterOffset];
					}
				}

				alpha = factor * alpha + bias;

				if (alpha > 255) {
					alpha = 255;
				} else if (alpha < 0) {
					alpha = 0;
				}

				resultPixelMap[byteOffset] = alpha;
			}
		}

		return resultPixelMap;
	}
}
