using _20263recetario.Context;
using _20263recetario.DTOs.Categories;
using _20263recetario.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _20263recetario.Controllers
{ 
        [ApiController]
        [Route("api/[controller]")]
        public class CategoriesController : ControllerBase
        {
            private readonly ApplicationDbContext context;//inyeccion de dependecias es como poner variables para usar
            private readonly IMapper mapper;

            public CategoriesController(ApplicationDbContext context, IMapper mapper)
            {
                this.context = context;
                this.mapper = mapper;
            }

            [HttpGet]
            public async Task<ActionResult<List<CategoryDtos>>> Get()
            {
                var categories = await context.Categories.ToListAsync();
                return mapper.Map<List<CategoryDtos>>(categories);//como consulta sql
            }

            [HttpGet("{id:int}")]
            public async Task<ActionResult<CategoryDtos>> Get(int id)//como este es uno solo no es necesario hacer una lista
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);//aqui seria una consulta de todo donde sea igual el id
                if(category == null)
                {
                    return NotFound();//no existe esa categoria
                }

                return mapper.Map<CategoryDtos>(category);//si existe hacemos un mapeo de esa categoria
            }
        [HttpPost]//endpoint para crear una categoria
        public async Task<ActionResult<CategoryDtos>> Create(CategoryCreateDtos categoryCreateDtos)
        {
            var category = mapper.Map<Category> (categoryCreateDtos);//haceos un mapeo de mi dto al modelo
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var response = mapper.Map<CategoryDtos>(category);
            return CreatedAtAction(nameof(Get), new {id = category.Id}, response);//mandamos a llamar 
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDtos categoryUpdateDtos)
        {
            var category = await context.Categories.FindAsync(id);//determinar que exista el Id
            if (category is null)
            {
                return NotFound();  
            }
            mapper.Map(categoryUpdateDtos, category); //hacemos el regreso del modelo al dto
            await context.SaveChangesAsync();//hacemos los cambios en la BD
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)//damos solo el registro de que se elimino ya que ya no podemos consultarlo
        {
            var category = await context.Categories.FindAsync(id);
            if (category is null)
            {
                return NotFound();
            }
            context.Categories.Remove(category);//aqui lo eliminamos del modelo
            await context.SaveChangesAsync();//hasta aqui se elimina de la base de datos
            return NoContent();//todo se elimino correctamente
        }
        }
    }

