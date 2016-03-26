# Beeh1ve Std Documentation

This document describes common abstractions that are provided by std

## Monads
There are dozens of examples of how a concept of monad may be described and most of them are correct. This document has a very specific definition of the concept, as the purpose of the document is strictly practical.

There are concepts of effects and effectful computations. The purpose of *effect* (or sometimes it may be called *context*) is to enhance a certain data type with certain capabilities. For example, the List effect implies that you have a collection of elements of a certain data type. This is usually expressed as List<DataType>

An idea of effectful computation makes the concept of effect a little bit wider, as it introduces an ability to perform computations within the effect. A good example of that is transformation of a list.

```
List<int> ints = ...
ints.Select(_ => _.ToString())
```
In fact, the ToString() operation is nothing more but a computation that is performed inside the List effect. In our particular case, the List effect abstracts creation of a list as result of each computation.

In general, every effect abstracts certain logic behind its computation.

There are three monadic effects that are provided by std.
- Maybe
- Try
- Future
