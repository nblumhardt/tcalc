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

The `master` branch implements this as a token-driven parser. You can also view the parser expressed as a pure-text
parser by switching to the [`text-parser` branch](https://github.com/nblumhardt/tcalc/tree/text-parser).

