import { Controller } from '@nestjs/common';
import { AnunciosService } from './anuncios.service';

@Controller('anuncios')
export class AnunciosController {
  constructor(private readonly anunciosService: AnunciosService) {}
}
