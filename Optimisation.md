Idées d'optimisation : http://www.dotnetperls.com/optimization

- Use Static methods. The reason for this is that to call an instance method, the instance reference must be resolved, to determine what method to call. Static methods do not use an instance reference.
If you look at the intermediate language, you will see that static methods can be invoked with fewer instructions.

- Avoid local variables
With 2.32 ns
Without 1.92 ns  (return ((_temp1 **10**_temp2) + 5 - **temp2) _2;)_

- Isolate rarely used parts of methods in separate methods. This makes the fast path in the called method more efficient, which can have a significant performance gain**

Nous, on utilise des jagged-array (http://www.dotnetperls.com/jagged-array) sans le savoir, ce qui semble être le mieux en l'occurrence. Toutefois, jeter uun oeil à :

-Flattened arrays

Using two-dimensional arrays in C# is relatively slow. However, you can explicitly create a one-dimensional array and access it through arithmetic that supposes it is a two-dimensional array.

- Changer les distributions pour que leur représentation interne soit un array de seuils et un array de T et pas un dictionary. GARDER la possibilité de les remplir avec un dictionary (merci les ctor :D )

- Vérifier qu'on accde pas à un dico.Keys:
Benchmark loops

Using foreach on KeyValuePairs is several times faster than using Keys. This is probably because the Keys collection is not used. KeyValuePair allows us to simply look through each pair one at a time. This avoids lookups and using the garbage-collected heap for storage.
Benchmark for KeyValuePair foreach loop

KeyValuePair: 125 ms
Note:         This loops through the pairs in the Dictionary.

Keys loop:    437 ms
Note:         This gets the Keys, then loops through them.
> It does another lookup for the value.

