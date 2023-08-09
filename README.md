# Кластеризация CLOPE на C#

[![Лицензия](https://img.shields.io/badge/license-MIT-blue.svg)](https://opensource.org/licenses/MIT)

Этот репозиторий содержит реализацию алгоритма кластеризации CLOPE на языке C# , используя принципы чистой архитектуры.

## Обзор

Алгоритм CLOPE предназначен для кластеризации данных путем группировки транзакций на основе их схожести. Эта реализация предоставляет функциональность для обработки и кластеризации данных с использованием алгоритма CLOPE, придерживаясь принципов чистой архитектуры.

## Установка и запуск примера

1. Клонируйте этот репозиторий на ваше локальное устройство.
2. Откройте файл решения `ClopeCSharp.sln` с помощью Visual Studio или вашей предпочитаемой среды разработки на C#.

## Использование

1. Откройте файл `Program.cs` в проекте `CLI`.
2. При необходимости настройте пути и параметры для вашего источника данных.
3. Запустите приложение. Оно будет читать данные, выполнять кластеризацию с использованием алгоритма CLOPE и выводить результаты.

```csharp
const double repulsion = 1.8;

// Создание источника данных
var fileDataSource = new FileDataReader(@"..\..\..\..\..\Mushroom_DataSet\agaricus-lepiota.data");

// Отображение идентификаторов транзакций на классы
var transactionIdToClassMap = new Dictionary<int, string>();
var mushroomOptions = new NormalizeOptions { TestDataColumn = 0 };
var mushroomData = new DataSourceNormalizer(fileDataSource, mushroomOptions, transactionIdToClassMap);

// Создание хранилища входных данных
var inputData = new TransactionsMemoryStorage(mushroomData.ToList());

// Создание хранилища кластеров
var clusterStorage = new ClusterStorage();

// Создание и запуск алгоритма CLOPE
var clopeAlgorithm = new ClopeAlgorithm(inputData, clusterStorage, repulsion);
var stopwatch = Stopwatch.StartNew();
clopeAlgorithm.Run();
stopwatch.Stop();
Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");

// Вывод результатов
var printUi = new DrawingReport(clusterStorage, transactionIdToClassMap);
printUi.Print();
```

## Архитектура проекта

Проект реализован в соответствии с принципами чистой архитектуры (по крайней мере попытка такая была), что способствует лучшей организации кода и разделению бизнес-логики от инфраструктуры.

## Лицензия

Этот проект лицензирован под MIT License - см. файл [LICENSE](LICENSE) для дополнительных деталей.

## Благодарности

Теоретическая основа была взята из следующих источников:
- [CLOPE на сайте Loginom](https://loginom.ru/blog/clope)
- [Реализация CLOPE на GitHub](https://github.com/Hedgehogues/CLOPE/blob/master/CLOPE_hedgehogues_urvanov.ipynb)
- [Алгоритм CLOPE на Algowiki](https://algowiki-project.org/ru/Участник:Артем_Карпухин/Алгоритм_CLOPE_кластеризации_категориальных_данных)

Также выражаем благодарность авторам репозитория [Go-CLOPE](https://github.com/realb0t/go-clope).

---