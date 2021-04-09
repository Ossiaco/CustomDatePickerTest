// Copyright (c) PlaceholderCompany. All rights reserved.

namespace Custom.DatePickerTest.Models
{
    using System;
    using ReactiveUI;

    /// <summary>
    /// A model base.
    /// </summary>
    ///
    /// <seealso cref="T:ReactiveUI.ReactiveObject"/>
    /// <seealso cref="T:Chorus.Graph.Vertices.IVertex"/>
    public abstract class ModelBase : ReactiveObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelBase"/> class.
        /// </summary>
        public ModelBase()
        {
            this.Id = Guid.Empty;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        ///
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        ///
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }
    }
}
