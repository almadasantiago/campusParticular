### SISTEMA CAMPUS VIRTUAL

## STACK

- **Backend:** ASP.NET Core sobre .NET 10
- **Frontend:** React + Vite + TypeScript + Redux Toolkit
- **Mobile:** a definir más adelante (probablemente React Native, reutilizando tipos con el frontend web)

## Backend


## Arquitectura

Docker y CI/CD quedan pospuestos hasta después del MVP funcional.

### Estructura de carpetas (frontend)

```
src/
├── store/
│   ├── store.ts
│   ├── slices/
│   └── hooks.ts
├── api/
├── types/
├── components/
├── pages/
└── routes/
```

## Decisiones técnicas relevantes

- **UUID como PK** en vez de autoincrement, pensando en una futura separación
  en microservicios (evita colisión de IDs entre servicios).
- **Borrado lógico (soft-delete)** en Usuario, Curso, Contenido y Entrega
  (campo `eliminadoEn: DateTime?`, null = activo). Borrado físico en entidades
  sin valor histórico (Mensaje, ForoRespuesta, Notificacion, DifusionLectura).

## Modelo de datos

Entidades principales:

- **Usuario** (rol fijo: ADMIN / PROFESOR / ESTUDIANTE). ADMIN es superusuario,
  puede operar como si fuera profesor en cualquier curso.
- **Curso** (ex-"Materia"): tiene `fechaInicio`/`fechaFin` en vez de período
  académico fijo, `cupoMaximo` opcional, admite co-dictado (varios profesores
  vía `CursoProfesor`).
- **Inscripcion**: flujo con estado `PENDIENTE` (solicitud) → `ACTIVA`
  (aprobada por profesor/admin) → `FINALIZADA`/`BAJA`/`RECHAZADA`.
- **Contenido** (material / video / lectura / tarea / documento de notas
  autogenerado).
- **Entrega**: versionada — cada nueva subida crea una fila (`version`,
  `esVigente`), preservando el historial completo. El profesor puede corregir
  la nota las veces que quiera.
- **ForoTema / ForoRespuesta**: por curso (sin foro global por ahora).
- **Mensaje**: 1 a 1.
- **Difusion / DifusionLectura**: comunicado de un profesor/admin a todo un
  curso, con tracking de lectura por destinatario (distinto de Mensaje 1 a 1).
- **Notificacion**: generada automáticamente por eventos del sistema, con
  campo `canal` (IN_APP ahora; preparado para EMAIL/PUSH a futuro).
- **Anuncio**: por curso o global (`cursoId` nullable).
- **Evento**: puntual o recurrente (regla de recurrencia calculada al vuelo,
  sin generar una fila por ocurrencia), con flag `esEvaluable` para habilitar
  la generación de documento de notas.
- **CalificacionEvento**: nota + observaciones por estudiante, asociada a un
  Evento evaluable (independiente de Entrega, para evaluaciones sin entrega
  digital, ej. examen presencial).

## Reglas de negocio

### Transversales

- Borrado lógico en Usuario, Curso, Contenido y Entrega (ver Decisiones técnicas).
- Borrado físico permitido en Mensaje, ForoRespuesta, Notificacion y DifusionLectura.
- Toda entidad con borrado lógico se excluye por defecto de los listados; un
  ADMIN puede consultar el historial completo.
- El ADMIN puede operar sobre cualquier curso como si fuera uno de sus
  profesores (crear/editar/eliminar contenido, aprobar inscripciones, corregir
  entregas).

### Usuario

- Un usuario no puede auto-eliminarse ni cambiarse el propio rol.
- Solo ADMIN puede cambiar el rol de un usuario o desactivarlo (`activo: false`).
- Un usuario desactivado no puede iniciar sesión, pero su historial
  (entregas, mensajes, foros) permanece visible para quienes ya interactuaron con él.
- Un PROFESOR no puede eliminar su propia cuenta si tiene
  cursos activos a su cargo sin otro co-profesor asignado.

### Curso

- Solo ADMIN o un PROFESOR asignado al curso pueden editarlo o eliminarlo.
- Un curso no puede eliminarse si tiene inscripciones `ACTIVA` sin antes
  finalizarlas o reasignarlas _(a confirmar: ¿se bloquea, o se permite con aviso?)_.
- El `cupoMaximo` no puede reducirse por debajo de la cantidad de inscripciones
  `ACTIVA` actuales.
- Eliminar un curso (soft-delete) no elimina su Contenido/Entregas/Foro: quedan
  accesibles en modo lectura para quienes ya participaron.

### Inscripcion

- Un estudiante solicita inscripción → queda en `PENDIENTE`.
- Solo un PROFESOR del curso o ADMIN puede aprobar (`ACTIVA`) o rechazar (`RECHAZADA`).
- No se puede aprobar una solicitud si ya se alcanzó el `cupoMaximo` del curso.
- Si un estudiante se da de baja y quiere re-inscribirse, se reutiliza la misma
  fila cambiando el estado (no se crea una nueva, por el `@@unique`).

### Contenido

- Solo PROFESOR del curso o ADMIN pueden crear/editar/eliminar contenido.
- Un Contenido de tipo TAREA no puede eliminarse si ya tiene Entregas asociadas (El unico caso en el que no exista mas es si se elimina el curso)
- La `fechaEntrega` de una tarea no puede modificarse a una fecha pasada si ya
  hay entregas realizadas antes del cambio.
- Un Contenido de tipo `DOCUMENTO_NOTAS` no puede crearse ni editarse
  manualmente — solo se genera a través del flujo de documento de notas (ver abajo).

### Entrega

- El estudiante puede volver a subir una entrega mientras no se haya superado
  la `fechaEntrega` del Contenido asociado; cada subida crea una nueva fila
  (`version` incremental) y marca la anterior `esVigente: false`.
- Una vez superada la `fechaEntrega`, el estudiante ya no puede crear nuevas
  versiones _(a confirmar: ¿excepción puntual tipo "entrega tardía permitida"?)_.
- El profesor puede corregir (nota + feedback) una entrega las veces que
  quiera, incluso después de haberla calificado por primera vez.
- Solo se califica la versión `esVigente: true`; las versiones anteriores
  quedan de solo lectura como historial.

### Foro

- Cualquier inscripto (ACTIVA) o profesor del curso puede crear temas y responder.
- Un usuario puede eliminar sus propias respuestas/temas; un PROFESOR o ADMIN
  puede eliminar cualquier respuesta/tema del curso (moderación).

### Mensajería y Difusión

- Un mensaje 1 a 1 solo puede eliminarlo (de su propia vista) quien lo envió o
  lo recibió _(a confirmar: eliminación "para mí", o para ambos?)_.
- Solo PROFESOR o ADMIN de un curso pueden enviar una Difusión a ese curso.
- Una Difusión ya enviada no puede editarse ni eliminarse (es un comunicado oficial).

### Notificaciones

- Generadas automáticamente por eventos del sistema (nueva tarea, respuesta en
  foro, cambio de estado de inscripción, etc.), nunca creadas manualmente.
- Un usuario solo puede marcar como leídas/eliminar sus propias notificaciones.

### Anuncio

- Solo PROFESOR del curso (si `cursoId` no es null) o ADMIN (si es global)
  pueden crear/eliminar anuncios.

### Evento

- Solo PROFESOR del curso o ADMIN pueden crear/editar/eliminar eventos de ese curso.
- Un evento recurrente, al eliminarse, elimina todas sus ocurrencias futuras
  (no existen como filas separadas, se calculan al vuelo).

### Generador de documento de notas (Evento)

- Solo un Evento con `esEvaluable: true` habilita la carga de `CalificacionEvento`
  y la generación del documento.
- Solo PROFESOR del curso o ADMIN pueden cargar/editar calificaciones y
  generar/regenerar el documento.
- El documento se puede generar en cuanto exista **al menos una**
  `CalificacionEvento` cargada para ese evento (no hace falta que estén
  completas todas las del curso; por ejemplo, si solo un estudiante rindió,
  se puede generar igual). No se puede generar si no hay ninguna nota cargada.
- Los estudiantes inscriptos sin nota cargada figuran como "pendiente" en la
  planilla del PDF.
- El documento generado es un `Contenido` de tipo `DOCUMENTO_NOTAS`,
  `generadoAutomaticamente: true`, visible para todos los inscriptos del curso
  igual que cualquier otro material. Formato: un único PDF con la planilla de
  notas de todo el curso (no uno individual por estudiante).
- Regenerar el documento (tras corregir o agregar una nota) es una acción
  manual (botón "Regenerar"), no automática: no se edita el PDF existente, se
  soft-elimina el `Contenido` anterior y se crea uno nuevo, preservando el
  historial de versiones generadas.
- Plantilla del PDF: fija/hardcodeada para el MVP (sin selección ni carga de
  plantillas personalizadas por ahora).

## Pendiente / próximos pasos

- [ ] Definir la estructura de carpetas del backend, el ORM y la base de datos
- [ ] Implementar el modelo de datos descrito arriba
- [ ] Resolver decisiones marcadas como "a confirmar" en Reglas de negocio
- [ ] Implementar las entidades y los DTOs
- [ ] Implementar autenticación y autorización por rol
- [ ] Definir estrategia de almacenamiento de archivos (entregas, PDFs generados)
- [ ] Docker (pospuesto para después del MVP)
- [ ] CI/CD (pospuesto)
- [ ] App mobile (pospuesto hasta tener la web funcionando)
