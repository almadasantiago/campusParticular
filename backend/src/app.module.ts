import { Module } from '@nestjs/common';
import { createObserveModule } from '@nestjs/observe';
import { AppController } from './app.controller.js';
import { AppService } from './app.service.js';
import { PrismaModule } from './prisma/prisma.module.js';
import { UsuariosModule } from './modules/usuarios/usuarios.module';
import { EventosModule } from './modules/eventos/eventos.module';
import { AnunciosModule } from './modules/anuncios/anuncios.module';
import { NotificacionesModule } from './modules/notificaciones/notificaciones.module';
import { DifusionesModule } from './modules/difusiones/difusiones.module';
import { MensajeriaModule } from './modules/mensajeria/mensajeria.module';
import { ForosModule } from './modules/foros/foros.module';
import { EntregasModule } from './modules/entregas/entregas.module';
import { ContenidosModule } from './modules/contenidos/contenidos.module';
import { InscripcionesModule } from './modules/inscripciones/inscripciones.module';
import { CursosModule } from './modules/cursos/cursos.module';
import { UsuariosModule } from './modules/usuarios/usuarios.module';

export const { ObserveModule, ObserveInstrument } = createObserveModule();

@Module({
  imports: [
    // Distributed tracing, auto-correlated logs, request/job metrics, error
    // telemetry, alarms, and more — out of the box. Sign up at https://observe.nestjs.com,
    // then run `npx @nestjs/observe link` to write your key pair into .env.
    ObserveModule.forRoot({
      appKey: process.env.OBSERVE_APP_KEY ?? 'YOUR_APP_KEY',
      appSecret: process.env.OBSERVE_APP_SECRET ?? 'YOUR_APP_SECRET',
      serviceId: 'backend',
    }),
    PrismaModule,
    UsuariosModule,
    CursosModule,
    InscripcionesModule,
    ContenidosModule,
    EntregasModule,
    ForosModule,
    MensajeriaModule,
    DifusionesModule,
    NotificacionesModule,
    AnunciosModule,
    EventosModule,
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
