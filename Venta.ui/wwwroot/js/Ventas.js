function addProducto() {
    // Simple JS to add product fields dynamically (for demonstration)
    var container = document.getElementById('productos-list');
    var index = container.children.length;
    var div = document.createElement('div');
    div.innerHTML = `<label>Producto:</label>
            <input type="text" name="Productos[${index}].Producto" required />
            <label>Cantidad:</label>
            <input type="number" name="Productos[${index}].Cantidad" min="1" required />`;
    container.appendChild(div);
}