# CitySim

Juego de simulación de ciudades hecho en Unity (C#).

**Autor:** Jehu Rodriguez (GitHub: JehuRodriguez)

## Cómo se juega
1. Elige un edificio con los botones de abajo o con las teclas 1 a 4.
2. Haz clic en el terreno para construirlo.
3. Las casas traen ciudadanos, que trabajan, ganan y gastan dinero.
4. El dinero de la ciudad crece con los impuestos, la producción y las ventas.

## Edificios
- Casa: da vivienda y cobra impuestos.
- Fábrica: da empleos y produce dinero.
- Tienda: da empleos y vende.
- Parque: cuesta mantenimiento.

## Conceptos usados
Clases, objetos, herencia, polimorfismo, clase abstracta (`Building`), interfaces (`IResidence`, `IWorkplace`), singleton (managers), observer (eventos), delegate, Action, Func y event.

## Diagrama de clases
![Diagrama de clases](Docs/DiagramaDeClases.png)