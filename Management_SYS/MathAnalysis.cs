using System;
using System.Collections.Generic;
using System.Linq;

namespace Management_SYS
{
    // Клас для збереження даних RFM одного клієнта
    public class RfmClientData
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }

        // Вхідні дані для математики
        public double Recency { get; set; }   // Днів з останньої покупки/контакту
        public double Frequency { get; set; } // Кількість покупок/контактів
        public double Monetary { get; set; }  // Сума грошей

        // Результат кластеризації (0 - Економ, 1 - Стандарт, 2 - VIP)
        public int ClusterId { get; set; }
        public string SegmentName { get; set; }
    }

    public class KMeansClustering
    {
        // Алгоритм K-Means (Метод k-середніх)
        public static List<RfmClientData> PerformClustering(List<RfmClientData> data, int k = 3)
        {
            if (data.Count < k) return data; // Недостатньо даних

            // 1. Нормалізація даних (щоб гроші не переважували давність)
            NormalizeData(data);

            // 2. Ініціалізація центроїдів (випадкові точки)
            var random = new Random();
            var centroids = new List<double[]>();
            for (int i = 0; i < k; i++)
            {
                var r = data[random.Next(data.Count)];
                centroids.Add(new double[] { r.Recency, r.Frequency, r.Monetary });
            }

            bool changed = true;
            int maxIterations = 100;
            int iter = 0;

            while (changed && iter < maxIterations)
            {
                changed = false;
                iter++;

                // 3. Призначення точок до найближчого кластера
                foreach (var point in data)
                {
                    int nearestCluster = 0;
                    double minDistance = double.MaxValue;

                    for (int i = 0; i < k; i++)
                    {
                        double dist = GetEuclideanDistance(point, centroids[i]);
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            nearestCluster = i;
                        }
                    }

                    if (point.ClusterId != nearestCluster)
                    {
                        point.ClusterId = nearestCluster;
                        changed = true;
                    }
                }

                // 4. Перерахунок центроїдів
                for (int i = 0; i < k; i++)
                {
                    var pointsInCluster = data.Where(p => p.ClusterId == i).ToList();
                    if (pointsInCluster.Count > 0)
                    {
                        centroids[i][0] = pointsInCluster.Average(p => p.Recency);
                        centroids[i][1] = pointsInCluster.Average(p => p.Frequency);
                        centroids[i][2] = pointsInCluster.Average(p => p.Monetary);
                    }
                }
            }

            // 5. Призначення назв сегментам (Інтерпретація)
            AssignSegmentNames(data, centroids);

            return data;
        }

        private static void NormalizeData(List<RfmClientData> data)
        {
            double maxR = data.Max(x => x.Recency) + 1; // +1 щоб уникнути ділення на 0
            double maxF = data.Max(x => x.Frequency) + 1;
            double maxM = data.Max(x => x.Monetary) + 1;

            // Тут ми просто змінюємо значення в пам'яті для розрахунку
            // В реальному проекті краще зберігати нормалізовані значення окремо
            foreach (var item in data)
            {
                // Інвертуємо Recency, бо чим менше днів, тим краще (1 - значення)
                // Але для простоти K-Means просто нормалізуємо
                item.Recency = item.Recency / maxR;
                item.Frequency = item.Frequency / maxF;
                item.Monetary = item.Monetary / maxM;
            }
        }

        private static double GetEuclideanDistance(RfmClientData p, double[] centroid)
        {
            return Math.Sqrt(
                Math.Pow(p.Recency - centroid[0], 2) +
                Math.Pow(p.Frequency - centroid[1], 2) +
                Math.Pow(p.Monetary - centroid[2], 2)
            );
        }

        private static void AssignSegmentNames(List<RfmClientData> data, List<double[]> centroids)
        {
            // Визначаємо, який кластер "найбагатший" (найбільше Monetary і Frequency)
            var clusterScores = centroids.Select((c, index) => new { Index = index, Score = c[1] + c[2] - c[0] }).OrderByDescending(x => x.Score).ToList();

            int bestCluster = clusterScores[0].Index;
            int midCluster = clusterScores.Count > 1 ? clusterScores[1].Index : -1;
            int lowCluster = clusterScores.Count > 2 ? clusterScores[2].Index : -1;

            foreach (var item in data)
            {
                if (item.ClusterId == bestCluster) item.SegmentName = "VIP Клієнт";
                else if (item.ClusterId == midCluster) item.SegmentName = "Активний";
                else item.SegmentName = "В зоні ризику";
            }
        }
    }
    public class PredictionModel
    {
        // Проста лінійна регресія: y = mx + b
        // Де x - номер місяця, y - сума продажів
        public static double PredictNextMonthSales(List<double> salesHistory)
        {
            if (salesHistory.Count < 2)
                return salesHistory.Count == 1 ? salesHistory[0] : 0; // Замало даних для прогнозу

            int n = salesHistory.Count;
            List<double> x = new List<double>(); // Час (1, 2, 3...)
            for (int i = 1; i <= n; i++) x.Add(i);

            double sumX = x.Sum();
            double sumY = salesHistory.Sum();
            double sumXY = 0;
            double sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                sumXY += x[i] * salesHistory[i];
                sumX2 += x[i] * x[i];
            }

            // Розрахунок коефіцієнтів (Slope m і Intercept b)
            double m = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double b = (sumY - m * sumX) / n;

            // Прогноз на наступний місяць (x = n + 1)
            double nextX = n + 1;
            double prediction = m * nextX + b;

            return prediction > 0 ? prediction : 0; // Прогноз не може бути від'ємним
        }
    }
}