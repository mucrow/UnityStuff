# Avoiding exception hiding in async C# code

hello friends who use Unity or C# - if you've ever been bitten by async/await code hiding an exception and you don't know why, i have a message for you:

after much testing, i can say with confidence that there's only one rule you need to make sure you don't break to prevent async code hiding an exception:

## the rule

`async Task` methods must be awaited. this includes methods with signatures like `async Task<T>`.

you don't need to do `await` right away - you can keep a Task around and `await` it later in the method to do other work before blocking execution of the caller.

this rule can also be simplified to "methods returning a `Task` should always be awaited". the **callee** does not need to be `async` to be `await`ed, although i don't think a non-`async` method returning a `Task` can cause an exception to be hidden directly.

## details

so let's say we're dealing with two methods, `First()` and `Second()`.

the `First()` method will call `Second()`.

here's what happens when `First()` is an `async void` method and `Second()` throws an exception:

| type signature | call to Second() | behaviour |
|----------------|------------------|-----------|
| `async Task Second()` | `await Second();` | :sparkles: exceptions are bubbled up |
| `async Task<T> Second()` | `await Second();` | :sparkles: exceptions are bubbled up |
| `async void Second()` | `await Second();` | :no_entry: compile error |
| `async Task Second()` | `Second();` | :skull_and_crossbones: exceptions are hidden |
| `async Task<T> Second()` | `Second();` | :skull_and_crossbones: exceptions are hidden |
| `async void Second()` | `Second();` | :sparkles: exceptions are bubbled up |

## what if i can't await

making explicit the cases above, there's only one remaining issue to address, which is "what do i do when i can't use `await` (i.e., `First()` is not `async`)"

this answer is probably obvious to some people reading this, but if you aren't aware, i think you are always able to do the following transformations:

`void First()` :arrow_right: `async void First()`

`void First()` :arrow_right: `async Task First()`

`SomeType First()` :arrow_right: `async Task<SomeType> First()`

this includes Unity lifecycle callbacks like `Start()` (e.g., `async void Start()` in a `MonoBehaviour` is fine and functions as expected !)

just make sure that if you refactor `First()` to return a `Task`, you `await` it in the calling method (;

hope this helps.
