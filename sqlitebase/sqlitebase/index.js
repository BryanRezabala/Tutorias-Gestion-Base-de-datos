const sqlite3 = require('sqlite3').verbose();
const prompt = require("prompt-sync")({ sigint: true });;
let sql = "";
let sql_m = "";
const db = new sqlite3.Database('base.db', sqlite3.OPEN_READWRITE, (error) => {
  if (error) {
    console.error(error);
  }
});

console.log(`Escribe la alternativa que corresponde: 
(a) Crear un usuario.
(b) Modificar un usuario.
(c) Eliminar un usuario.
(d) Crear un Rol.
(e) Asignar un Rol a un Usuario.
(f) Consultar los usuarios creados.
(g) Consultar los roles creados. `);
let arg = prompt("Ingresa tu alternativa:");
switch (arg) {
  case 'a':
    crear_usu();
    break;
  case 'b':
    mod_usu();
    break;

  case 'c':
    eliminar_usu();
    break;

  case 'd':
    crear_rol();
    break;
  case 'e':
    esco_user();
    break;
  case 'f':
    consul_usu();
    break;
  case 'g':
    consul_roles();
    break;
  default:
    alert('Un valor desconocido');
    break;
}

function crear_rol() {
  const rol = {
    descr: ''
  };
  rol.descr = prompt('Ingrese rol: ');
  sql = `INSERT INTO ROL_USER (descripcion_rol,estado)
  VALUES(?,?);`;

  db.run(sql, [rol.descr,1], (error) => {
    if (error) {
      console.error(error);
    } else {
      console.log("Rol creado satisfactoriamente");
    }
  });
}

function crear_usu() {
  const usuario = {
    nom: '',
    contra: ''
  };
  usuario.nom = prompt('Ingrese su usuario');
  usuario.contra = prompt('Ingrese una contraseña');
  sql = `INSERT INTO USER (nombre_user,contraseña_user,estado)
VALUES(?,?,?);`;

  db.run(sql, [usuario.nom, usuario.contra, 1], (error) => {
    if (error) {
      console.error(error);
    } else {
      console.log("Usuario creado satisfactoriamente");
    }
  });
}
function mod_usu() {
  const usuario_mod = {
    nom: '',
    contra: ''
  };
  let id_user = "";
  usuario_mod.nom = prompt('Ingrese su usuario');
  sql = `select * from user where user.nombre_user = ?;`;
  db.get(sql, [usuario_mod.nom], (error, rows) => {
    if (error) {
      console.error(error);
    } else {
      id_user = rows.id;
      mod_usuario(id_user);
    }
  });
}
function mod_usuario(a) {
  const usuario_actualizar = {
    nom: '',
    contra: ''
  };
  usuario_actualizar.nom = prompt('Ingrese nuevo usuario');
  usuario_actualizar.contra = prompt('Ingrese nueva contraseña');
  sql = `UPDATE USER
  SET nombre_user = ?,
  contraseña_user = ?
  WHERE
      id = ?;`;
  db.run(sql, [usuario_actualizar.nom, usuario_actualizar.contra, a], (error) => {
    if (error) {
      console.error(error);
    } else {
      console.log("Usuario actualizado");
    }
  });
}
function eliminar_usu() {
  const usuario_mod = {
    nom: '',
    contra: ''
  };
  let id_user = "";
  usuario_mod.nom = prompt('Ingrese su usuario ');
  sql = `select * from user where user.nombre_user = ?;`;
  db.get(sql, [usuario_mod.nom], (error, rows) => {
    if (error) {
      console.error(error);
    } else {
      id_user = rows.id;
      elim(id_user);
    }
  });
}
function elim(b) {
   sql = `DELETE FROM USER WHERE id= ?`;
  db.run(sql, [b], (error) => {
    if (error) {
      console.error(error);
    } else {
      console.log("Usuario eliminado satisfactoriamente");
    }
  });
}
function esco_user() {
  const usuario_mod = {
    nom: '',
    contra: ''
  };
  let id_user = "";
  usuario_mod.nom = prompt('Ingrese su usuario');
  sql = `select * from user where user.nombre_user = ?;`;
  db.get(sql, [usuario_mod.nom], (error, rows) => {
    if (error) {
      console.error(error);
    } else {
      id_user = rows.id;
      mod_usuario_rol(id_user);
    }
  });
}
function mod_usuario_rol(c) {
  const rol_desc = {
    desc: ''
  };
  let rol_num = '';
  rol_desc.desc = prompt('Ingrese ROL (admin o usuario)');
  if(rol_desc.desc == 'admin'){
    rol_num = 1;
  }else{
    rol_num = 2;
  }
  sql = `UPDATE USER
  SET id_rol = ?
  WHERE
      id = ?;`;
  db.run(sql, [rol_num, c], (error) => {
    if (error) {
      console.error(error);
    } else {
      console.log("Se le asigno el rol de " + rol_desc.desc);
    }
  });
}
function consul_usu() {

  sql = `select user.nombre_user from USER where user.estado = '1';`;
  db.all(sql, (error, rows) => {
    if (error) {
      console.error(error);
    } else {
      /* id_user = rows.id;
      mod_usuario(id_user); */
      console.log(rows);
    }
  });
}
function consul_roles() {

  sql = `select ROL_USER.descripcion_rol from ROL_USER where ROL_USER.estado = '1';`;
  db.all(sql, (error, rows) => {
    if (error) {
      console.error(error);
    } else {
      /* for( let i = 0 ; i < rows.length ; i++){
        console.log("ROL : " + rows.descripcion_rol[i])
      } */
      /* id_user = rows.id;
      mod_usuario(id_user); */
      console.log(rows);
    }
  });
}