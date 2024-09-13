# Проект "Subway Surfers" 

В этом проекте реализован паттерн **Service Locator**, основанный на механике популярной игры "Subway Surfers". 

## Основные скрипты

В проекте присутствуют три основных скрипта:

- `RoadSpawner.cs`
- `PlayerMovementController.cs`
- `InputController.cs`

С помощью паттерна Service Locator решаются проблемы зависимостей между этими скриптами.

## Скрипт RoadSpawner.cs

В скрипте `RoadSpawner.cs` есть метод:

```csharp
private void ReloadRoad()
{
    GameObject road = _roads.Peek();
    Vector3 playerPosition = _playerMovementController.GetPlayerPosition();

    if (Vector3.Distance(playerPosition, road.transform.position) > _lengthRoad)
    {
        road = _roads.Dequeue();
        Vector3 position = road.transform.position;
        position.z += _countSpawns * _lengthRoad;
        road.transform.position = position;
        _roads.Enqueue(road);
    }
}
```
Он используется для отслеживания, прошел ли игрок дорогу. Если он преодолел дистанцию, дорога должна переместиться в самый конец списка доступных дорог. Однако возникает проблема: скрипт обязан знать о позиции игрока.

## Скрипт PlayerMovementController.cs
В дополнение, в скрипте PlayerMovementController необходимо знать, какие клавиши нажаты. Считывание нажатий клавиш было перенесено в InputController, так как текущий скрипт отвечает за другую ответственность и не должен заниматься контролем ввода пользователя. Это создает прямую зависимость между скриптом, который управляет движением игрока, и скриптом, который обрабатывает ввод.

## Заключение
Исходя из вышесказанного, была внедрена реализация паттерна Service Locator, чтобы передавать необходимые зависимости для каждого компонента через Loader.
