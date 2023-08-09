using System.Diagnostics;
using Application;
using CLI;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Repository;

const double repulsion = 2.6;

var fileDataSource = new FileDataReader(@"..\..\..\..\..\Mushroom_DataSet\agaricus-lepiota.data");

var transactionIdToClassMap = new Dictionary<int, string>();
var mushroomOptions = new NormalizeOptions { TestDataColumn = 0 };

var mushroomData = new DataSourceNormalizer(fileDataSource, mushroomOptions, transactionIdToClassMap);

var inputData = new TransactionsMemoryStorage(mushroomData.ToList());
var clusterStorage = new ClusterStorage();

var clopeAlgorithm = new ClopeAlgorithm(inputData, clusterStorage, repulsion);

var stopwatch = Stopwatch.StartNew();

clopeAlgorithm.Run();

stopwatch.Stop();
Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} ms");

var printUi = new DrawingReport(clusterStorage, transactionIdToClassMap);
printUi.Print();