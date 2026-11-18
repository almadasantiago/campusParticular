import { Controller } from '@nestjs/common';
import { MensajeriaService } from './mensajeria.service';

@Controller('mensajeria')
export class MensajeriaController {
  constructor(private readonly mensajeriaService: MensajeriaService) {}
}
