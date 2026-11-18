import { Controller } from '@nestjs/common';
import { EventosService } from './eventos.service';

@Controller('eventos')
export class EventosController {
  constructor(private readonly eventosService: EventosService) {}
}
