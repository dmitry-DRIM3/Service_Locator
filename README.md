# Проект "Service Locator Example" 

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
В скрипте PlayerMovementController необходимо знать, какие клавиши нажаты.
```csharp
//В этом методе реализована функция для горизонтального движения игрока:
private void MoveHorizontally()
{
    Vector3 position = transform.position;
    position.x = Mathf.Lerp(position.x, _positionHorizontal.x, _speedHorizontal * Time.deltaTime);
    
    transform.position = position;

    float step;
    if (_inputController.GetMoveRightButtonDown())
    {
        step = _stepSizeHorizontal;
    }
    else if (_inputController.GetMoveLeftButtonDown())
    {
        step = -_stepSizeHorizontal;
    }
    else
    {
        return;
    }
   
    _positionHorizontal.x += step;
    _positionHorizontal.x = Mathf.Clamp(_positionHorizontal.x, _minX, _maxX);       
}
```
Считывание нажатий клавиш было перенесено в InputController, так как текущий скрипт отвечает за другую ответственность и не должен заниматься контролем ввода пользователя.
## Скрипт InputController.cs
```csharp
public class InputController : MonoBehaviour, IInputService
{
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _leftKey;
    [SerializeField] private KeyCode _rightKey;

    public bool GetJumpButtonDown()
    {
        return Input.GetKeyDown(_jumpKey);
    }

    public bool GetMoveLeftButtonDown()
    {
        return Input.GetKeyDown(_leftKey);
    }

    public bool GetMoveRightButtonDown()
    {
        return Input.GetKeyDown(_rightKey);
    }
}
//Использование InputController позволяет более четко разделить ответственность и улучшить читаемость кода.
```
Это создает прямую зависимость между скриптом, который управляет движением игрока, и скриптом, который обрабатывает ввод.

## Заключение
Исходя из вышесказанного, была внедрена реализация паттерна Service Locator, чтобы передавать необходимые зависимости для каждого компонента через Loader.
## Скрипт ServiceLocatorLoadMain.cs
```csharp
public class ServiceLocatorLoadMain : MonoBehaviour
{
    [SerializeField] private PlayerMovementController _playerMovementController;
    [SerializeField] private InputController _inputController;

    private void Awake()
    {
        ServiceLocator.RegisterService<PlayerMovementController>(_playerMovementController);
        ServiceLocator.RegisterService<InputController>(_inputController);     
    }
}
```
