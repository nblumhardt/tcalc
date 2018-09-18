# `tcalc`

A series of C# parser construction examples, using [Superpower](https://github.com/datalust/superpower).

This repository implements a toy language for simple calculations over durations expressed
in days (`d`), hours (`h`), minutes (`m`), seconds (`s`), or milliseconds (`ms`), along with floating point numbers:

```
tcalc> (1h - 50m) * 3
00:30:00

tcalc> 7d / 350ms
1728000

```

The `text-parser` branch implements this as a character-driven parser. You can also view the parser expressed as a token
parser by switching to the [`master` branch](https://github.com/nblumhardt/tcalc/).

