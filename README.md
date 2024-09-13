# Проект "Service Locator Example" 

В этом проекте реализован паттерн **Service Locator**, основанный на механике популярной игры "Subway Surfers".

Прежде чем рассмотреть применение хотелось бы затронуть слова известного разработчика "Martin Pauler" о паттерне.

## Паттерн Service Locator по мнению Мартина Паулера
Мартин Паулер, известный разработчик и автор, в своих работах о паттерне Service Locator подчеркивает, что этот паттерн следует использовать с осторожностью. Он предлагает несколько рекомендаций и критических замечаний о Service Locator:

1. **Скрытые зависимости**: Использование Service Locator может привести к неявным зависимостям, что усложняет понимание кода и его сопровождение. Лучше явно передавать зависимости через конструкторы.

2. **Тестирование**: Service Locator может усложнить юнит-тестирование, поскольку объекты зависят от глобального состояния. Более предпочтительным методом является использование Dependency Injection (DI).

3. **Проблемы с проектированием**: Service Locator может привести к менее чистой архитектуре и является примером антипаттерна, если используется неправильно. Он может затруднить масштабирование и рефакторинг кода.

4. **Альтернатива**: Вместо Service Locator он рекомендует использовать DI-контейнеры, которые обеспечивают более явное управление зависимостями и делают код более прозрачным.

Таким образом, хотя Service Locator может быть полезен в некоторых контекстах, важно оценить все преимущества и недостатки и рассмотреть более современные подходы, такие как Dependency Injection.
Вы можете ознакомиться с [моей реализацией DI](https://github.com/dmitry-DRIM3/Dependency-Injection-Example)
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
//В этом примере реализованы методы для прыжка и горизонтального движения игрока :
private void MoveHorizontally()
{
    Vector3 position = transform.position;
    position.x = Mathf.Lerp(position.x, _positionHorizontal.x, _speedHorizontal * Time.deltaTime);
    
    transform.position = position;

    float step;
    if (_inputController.MoveRightButtonDown)
    {
        step = _stepSizeHorizontal;
    }
    else if (_inputController.MoveLeftButtonDown)
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

private void Jump()
{     
    if (_inputController.JumpButtonDown && _isGrounded)
    {
        _inputController.ResetJumpRequest();
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }   
}
```
Считывание нажатий клавиш было перенесено в InputController, так как текущий скрипт отвечает за другую ответственность и не должен заниматься контролем ввода пользователя.
## Скрипт InputController.cs
```csharp
public class InputController : MonoBehaviour, IService
{
    public bool JumpButtonDown => _jumpButtonDown;
    public bool MoveLeftButtonDown => _moveLeftButtonDown;
    public bool MoveRightButtonDown => _moveRightButtonDown;

    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private KeyCode _leftKey;
    [SerializeField] private KeyCode _rightKey;

    private bool _jumpButtonDown;
    private bool _moveLeftButtonDown;
    private bool _moveRightButtonDown;

    public void ResetJumpRequest()
    {
        _jumpButtonDown = false;
    }

    private void Update()
    {
        _jumpButtonDown = !_jumpButtonDown ? Input.GetKeyDown(_jumpKey) : true;
        _moveLeftButtonDown = Input.GetKeyDown(_leftKey);
        _moveRightButtonDown =  Input.GetKeyDown(_rightKey);
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
